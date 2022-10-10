import { createRouter, createWebHistory } from "vue-router"
import MenuPage from "./pages/Menu/MenuPage.vue"
import NotFoundPage from "./pages/NotFound/NotFoundPage.vue"
import MapsPage from "./pages/Maps/MapsPage.vue"
import CharactersPage from "./pages/Characters/CharactersPage.vue"
import HeroesPage from "./pages/Heroes/HeroesPage.vue"
import AddHeroPage from "./pages/Heroes/AddHeroPage.vue"
import EditHeroPage from "./pages/Heroes/EditHeroPage.vue"
import ViewHeroPage from "./pages/Heroes/ViewHeroPage.vue"
import EnemiesPage from "./pages/Enemies/EnemiesPage.vue"
import AddEnemyPage from "./pages/Enemies/AddEnemyPage.vue"
import EditEnemyPage from "./pages/Enemies/EditEnemyPage.vue"
import ViewEnemyPage from "./pages/Enemies/ViewEnemyPage.vue"

const routes = [
    {
        path: '/',
        name: 'menu',
        component: MenuPage
    },
    {
        path: '/maps',
        name: 'all-maps',
        component: MapsPage
    },
    {
        path: '/characters',
        name: 'characters',
        component: CharactersPage
    },
    // --- heroes ---
    {
        path: '/heroes',
        name: 'all-heroes',
        component: HeroesPage
    },
    {
        path: '/heroes/add',
        name: 'add-hero',
        component: AddHeroPage
    },
    {
        path: '/heroes/edit/:heroId',
        name: 'edit-hero',
        component: EditHeroPage
    },
    {
        path: '/heroes/view/:heroId',
        name: 'view-hero',
        component: ViewHeroPage
    },
    // --- enemies ---
    {
        path: '/enemies',
        name: 'all-enemies',
        component: EnemiesPage
    },
    {
        path: '/enemies/add',
        name: 'add-enemy',
        component: AddEnemyPage
    },
    {
        path: '/enemies/edit/:enemyId',
        name: 'edit-enemy',
        component: EditEnemyPage
    },
    {
        path: '/enemies/view/:enemyId',
        name: 'view-enemy',
        component: ViewEnemyPage
    },
    // --- not found page ---
    { 
        path: "/:catchAll(.*)",
        name: 'not-found',
        component: NotFoundPage
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;