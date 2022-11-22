export default class StoryService {
    constructor(enemies, enemiesKilled) {
        this.enemies = enemies; // enemy -> quantity, name, category, id, difficulty
        this.enemiesKilled = enemiesKilled; // enemyKilled -> quantity, name, category, id, difficulty
        this.currentEnemy = null;
        this.storyState = storyStates.Archer;
        this.selectStoryState();
        this.storyTextShow = [];
        this.inputHandler = new InputHandler();
    }

    addEnemyKilled(enemy) {
        let enemyExists = this.enemiesKilled.find(e => e.enemy.id === enemy.id);

        if (!enemyExists) {
            enemyExists = this.enemies.find(e => e.enemy.id === enemy.id);
            
            if (!enemyExists) {
                throw new Error(`Enemy with id ${enemy.id} doesnt exists`);
            }

            this.enemiesKilled.push({ ...enemyExists, quantity: 1 });
            return;
        } else {
            enemyExists.quantity++;
        }

        this.selectStoryState();
    }

    showStoryText(context) {
        if (this.storyState === storyStates.Archer) {
            if (this.storyTextShow.find(s => s === storyStates.Archer)) {
                return;
            }

            const text = begin(this.enemies, this.enemiesKilled.find(e => e.category === 'Archer'));
            drawBubble(context, text);
            if (this.inputHandler.key === 'Escape') {
                this.storyTextShow.push(storyStates.Archer);
                document.dispatchEvent(new CustomEvent('storyAccepted', { detail: this.storyState }));
            }
        } else if (this.storyState === storyStates.Knight) {
            if (this.storyTextShow.find(s => s === storyStates.Knight)) {
                return;
            }

            const text = afterArchers(this.enemies, this.enemiesKilled.find(e => e.category === 'Knight'));
            drawBubble(context, text);
            if (this.inputHandler.key === 'Escape') {
                this.storyTextShow.push(storyStates.Knight);
                document.dispatchEvent(new CustomEvent('storyAccepted', { detail: this.storyState }));
            }
        } else if (this.storyState === storyStates.Dragon) {
            if (this.storyTextShow.find(s => s === storyStates.Dragon)) {
                return;
            }

            const text = afterKnights(this.enemies, this.enemiesKilled.find(e => e.category === 'Dragon'));
            drawBubble(context, text);
            if (this.inputHandler.key === 'Escape') {
                this.storyTextShow.push(storyStates.Dragon);
                document.dispatchEvent(new CustomEvent('storyAccepted', { detail: this.storyState }));
            }
        } else if (this.storyState === storyStates.End) {
            if (this.storyTextShow.find(s => s === storyStates.End)) {
                return;
            }

            const text = afterDragonsEnd();
            drawBubble(context, text);
        }
    }

    getArchers() {
        if (!this.archers) {
            this.archers = this.enemies.filter(e => e.enemy.category === 'Archer');
        }

        return this.archers[0];
    }

    getKnights() {
        if (!this.knights) {
            this.knights = this.enemies.filter(e => e.enemy.category === 'Knight');
        }

        return this.knights[0];
    }

    getDragons() {
        if (!this.dragons) {
            this.dragons = this.enemies.filter(e => e.enemy.category === 'Dragon');
        }

        return this.dragons[0];
    }

    selectStoryState() {
        const archers = this.getArchers();
        
        if (archers) {
            const archersQuantity = archers.quantity;
            const archersKilledQuantity = this.enemiesKilled.find(e => e.enemy.category === 'Archer')?.quantity ?? 0;

            if (archersKilledQuantity >= archersQuantity) {
                this.storyState = storyStates.Knight;
            } else {
                this.storyState = storyStates.Archer;
                return;
            }
        }

        const knights = this.getKnights();

        if (knights) {
            const knightsQuantity = knights.quantity;
            const knightsKilledQuantity = this.enemiesKilled.find(e => e.enemy.category === 'Knight')?.quantity ?? 0;

            if (knightsKilledQuantity >= knightsQuantity) {
                this.storyState = storyStates.Dragon;
            } else {
                this.storyState = storyStates.Knight;
                return;
            }
        }

        const dragons = this.getDragons();

        if (dragons) {
            const dragonsQuantity = dragons.quantity;
            const dragonsKilledQuantity = this.enemiesKilled.find(e => e.enemy.category === 'Dragon')?.quantity ?? 0;
            
            if (dragonsKilledQuantity >= dragonsQuantity) {
                this.storyState = storyStates.End;
            } else {
                this.storyState = storyStates.Dragon;
                return;
            }
        } else {
            this.storyState = storyStates.End;
        }
    }

    getNextEnemy() {
        if (this.enemiesKilled.length === 0) {
            this.currentEnemy = this.enemies.find(e => e.enemy.category === 'Archer');
            setEnemyName(this.currentEnemy.enemy.name);
            return this.currentEnemy;
        }

        const enemyKilled = this.enemiesKilled.find(ek => ek.enemy.id === this.currentEnemy.enemy.id);
        console.log(enemyKilled,this.currentEnemy, this.enemiesKilled);
        if (enemyKilled.quantity < this.currentEnemy.quantity) {
            return this.currentEnemy;
        }
        
        const nextEnemy = this.enemies.find(e => this.enemiesKilled.every(ek => ek.enemy.id !== e.enemy.id) && e.enemy.category === this.storyState);
        this.currentEnemy = nextEnemy;
        setEnemyName(nextEnemy.enemy.name);

        if (!nextEnemy) {
            throw new Error(`There is no new enemies for state ${this.storyState}`);
        }

        return nextEnemy;
    }
}

const setEnemyName = (name) => {
    document.querySelector('#enemyName').innerHTML = name;
}

const begin = (enemies, enemiesKilled) => {
    const text = [];
    text.push('You are the best hero who is on the way to kill the dragon\n');
    text.push('Dragon wants to destroy your kingdom\n');
    text.push(`On your way to the lairs of the dragons, there are couple of archers ${enemies.find(e => e.enemy.category === 'Archer').quantity}\n`);
    text.push('And they dont want to let you go without a fight\n');
    
    if (enemiesKilled) {
        text.push(`You killed ${enemiesKilled.quantity} Archers \n`);
    }
    
    text.push('Press Escape to continue ...');
    return text.join("");
}

const afterArchers = (enemies, enemiesKilled) => {
    const text = [];
    text.push('Archers dont want to duel you. Well Done! You continue on to the dragons lair!\n');
    text.push('However the dragons hired some knights to defense their lairs from people like you\n');
    text.push('Watch out!\n');
    text.push(`There are ${enemies.find(e => e.enemy.category === 'Knight').quantity} of them that have found out about your quest.\n`);
    text.push('And they dont want to let you go without a fight\n');
    
    if (enemiesKilled) {
        text.push(`You killed ${enemiesKilled.quantity} Knights\n`);
    }

    text.push('Press Escape to continue ...');
    return text.join("");
}

const afterKnights = (enemies, enemiesKilled) => {
    const text = [];
    text.push('Congrats you killed them, you continue on your journey!\n');
    text.push('You are close to the dragon lairs.\n');
    text.push("It's hot and dangerous in there.\n");
    text.push(`There are ${enemies.find(e => e.enemy.category === 'Dragon').quantity} dragons.\n`);
    text.push('The time has come to end the dragons rampage!\n');
    
    if (enemiesKilled) {
        text.push(`You killed ${enemiesKilled.quantity} Dragons\n`);
    }

    text.push('Press Escape to continue ...');
    return text.join("");
}

const afterDragonsEnd = () => {
    const text = [];
    text.push('You killed the dragons and saved the kingdom!\n');
    text.push('Congrats!\n');
    return text.join("");
}

const drawBubble = (context, text) => {
    const lineHeight = 15;
    const lines = text.split('\n');
    let maxLength = 0;
    let h = lines.length * lineHeight;

    for (let i = 0; i < lines.length; i++) {
        h += 5;
        if (lines[i].length > maxLength) {
            maxLength = lines[i].length;
        }
    }
    h += 5;
    const w = maxLength * 9.5;
    const x = context.width/2 - w/2;
    const y = 10;
    const radius = 20;
    const r = x + w;
    const b = y + h;
    context.beginPath();
    context.strokeStyle="black";
    context.lineWidth="2";
    context.moveTo(x+radius, y);
    context.lineTo(r-radius, y);
    context.quadraticCurveTo(r, y, r, y+radius);
    context.lineTo(r, y+h-radius);
    context.quadraticCurveTo(r, b, r-radius, b);
    context.lineTo(x+radius, b);
    context.quadraticCurveTo(x, b, x, b-radius);
    context.lineTo(x, y+radius);
    context.quadraticCurveTo(x, y, x+radius, y);
    context.stroke();

    context.font = '15px Courier New';
    context.fillStyle = 'white';
    for (let i = 0; i < lines.length; i++) {
        context.fillText(lines[i], x + 15, y + 25 + (i * lineHeight));
    }
}

export const storyStates = { Knight: 'Knight', Archer: 'Archer', Dragon: 'Dragon', End: 'End' };

class InputHandler {
    constructor() {
        this.key = null;
        window.addEventListener('keydown', e => {
            console.log(e.key);
            if (e.key === 'Escape') {
                this.key = e.key;
            } else if (e.key === '1') {
                this.key = e.key;
            }
        });

        window.addEventListener('keyup', e => {
            if (e.key === 'Escape') {
                this.key = null;
            } else if (e.key === '1') {
                this.key = null;
            }
        });
    }
}
