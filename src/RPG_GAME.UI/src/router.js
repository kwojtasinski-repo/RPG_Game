import { createRouter, createWebHistory } from "vue-router";
import MenuPage from "./pages/Menu/MenuPage.vue"
import NotFoundPage from "./pages/NotFound/NotFoundPage.vue"
import MapsPage from "./pages/Maps/MapsPage.vue"
import CharactersPage from "./pages/Characters/CharactersPage.vue"
import HeroesPage from "./pages/Heroes/HeroesPage.vue"
import AddHeroPage from "./pages/Heroes/AddHeroPage.vue"
import EditHeroPage from "./pages/Heroes/EditHeroPage.vue"
import ViewHeroPage from "./pages/Heroes/ViewHeroPage.vue"
import EnemiesPage from "./pages/Enemies/EnemiesPage.vue"

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
        path: '/heroes/edit/:id',
        name: 'edit-hero',
        component: EditHeroPage
    },
    {
        path: '/heroes/view/:id',
        name: 'view-hero',
        component: ViewHeroPage
    },
    // --- enemies ---
    {
        path: '/enemies',
        name: 'all-enemies',
        component: EnemiesPage
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