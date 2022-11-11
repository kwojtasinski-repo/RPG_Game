export default class StoryService {
    constructor(enemies, enemiesKilled) {
        this.enemies = enemies; // enemy -> quantity, name, category, id, difficulty
        this.enemiesKilled = enemiesKilled; // enemyKilled -> quantity, name, category, id, difficulty
        this.storyState = storyStates.Archer;
        this.selectStoryState();
        this.storyTextShow = [];
        this.inputHandler = new InputHandler();
    }

    addEnemyKilled(enemy) {
        this.enemiesKilled.push(enemy);
        this.selectStoryState();
    }

    showStoryText(context) {
        if (this.storyState === storyStates.Archer) {
            if (this.storyTextShow.find(s => s === storyStates.Archer)) {
                return;
            }

            const text = begin(this.enemies, this.enemiesKilled.filter(e => e.category === 'Archer'));
            drawBubble(context, text);
            if (this.inputHandler.key === 'Escape') {
                this.storyTextShow.push(storyStates.Archer);
            }
        } else if (this.storyState === storyStates.Knight) {
            if (this.storyTextShow.find(s => s === storyStates.Knight)) {
                return;
            }

            const text = afterArchers(this.enemies, this.enemiesKilled.filter(e => e.category === 'Knight'));
            drawBubble(context, text);
            if (this.inputHandler.key === 'Escape') {
                this.storyTextShow.push(storyStates.Knight);
            }
        } else if (this.storyState === storyStates.Dragon) {
            if (this.storyTextShow.find(s => s === storyStates.Dragon)) {
                return;
            }

            const text = afterKnights(this.enemies, this.enemiesKilled.filter(e => e.category === 'Dragon'));
            drawBubble(context, text);
            if (this.inputHandler.key === 'Escape') {
                this.storyTextShow.push(storyStates.Dragon);
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
            this.archers = this.enemies.filter(e => e.category === 'Archer');
        }

        return this.archers;
    }

    getKnights() {
        if (!this.knights) {
            this.knights = this.enemies.filter(e => e.category === 'Knight');
        }

        return this.knights;
    }

    getDragons() {
        if (!this.dragons) {
            this.dragons = this.enemies.filter(e => e.category === 'Dragon');
        }

        return this.dragons;
    }

    selectStoryState() {
        // eslint-disable-next-line
        debugger;
        const archers = this.getArchers().length;
        const archersKilled = this.enemiesKilled.filter(e => e.category === 'Archers').length;

        if (archersKilled >= archers) {
            this.storyState = storyStates.Knight;
        } else {
            this.storyState = storyStates.Archer;
            return;
        }

        const knights = this.getKnights().length;
        const knightsKilled = this.enemiesKilled.filter(e => e.category === 'Knight').length;

        if (knightsKilled >= knights) {
            this.storyState = storyStates.Dragon;
        } else {
            this.storyState = storyStates.Knight;
            return;
        }

        const dragons = this.getDragons().length;
        const dragonsKilled = this.enemiesKilled.filter(e => e.category === 'Dragon').length;
        
        if (dragonsKilled >= dragons) {
            this.storyState = storyStates.End;
        } else {
            this.storyState = storyStates.Dragon;
            return;
        }
    }
}

const begin = (enemies, enemiesKilled) => {
    const text = [];
    text.push('You are the best hero who is on the way to kill the dragon\n');
    text.push('Dragon wants to destroy your kingdom\n');
    text.push(`On your way to the lairs of the dragons, there are couple of archers ${enemies.filter(e => e.category === 'Archer').length}\n`);
    text.push('And they dont want to let you go without a fight\n');
    
    if (enemiesKilled.length > 0) {
        const archersKilled = enemiesKilled.filter(e => e.category === 'Archer').length;
        text.push(`You killed ${archersKilled} Archers \n`);
    }
    
    text.push('Press Escape to continue ...');
    return text.join("");
}

const afterArchers = (enemies, enemiesKilled) => {
    const text = [];
    text.push('Archers dont want to duel you. Well Done! You continue on to the dragons lair!\n');
    text.push('However the dragons hired some knights to defense their lairs from people like you\n');
    text.push('Watch out!\n');
    text.push(`There are ${enemies.filter(e => e.category === 'Knight').length} of them that have found out about your quest.\n`);
    text.push('And they dont want to let you go without a fight\n');
    
    if (enemiesKilled.length > 0) {
        const knightsKilled = enemiesKilled.filter(e => e.category === 'Knight').length;
        text.push(`You killed ${knightsKilled} Knights\n`);
    }

    text.push('Press Escape to continue ...');
    return text.join("");
}

const afterKnights = (enemies, enemiesKilled) => {
    const text = [];
    text.push('Congrats you killed them, you continue on your journey!\n');
    text.push('You are close to the dragon lairs.\n');
    text.push("It's hot and dangerous in there.\n");
    text.push(`There are ${enemies.filter(e => e.category === 'Dragon').length} dragons.\n`);
    text.push('The time has come to end the dragons rampage!\n');
    
    if (enemiesKilled.length > 0) {
        const dragonsKilled = enemiesKilled.filter(e => e.category === 'Dragon').length;
        text.push(`You killed ${dragonsKilled} Knights\n`);
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

const storyStates = { Knight: 'Knight', Archer: 'Archer', Dragon: 'Dragon', End: 'End' };

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
