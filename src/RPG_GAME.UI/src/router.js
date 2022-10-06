import { createRouter, createWebHistory } from "vue-router";
import MenuPage from "./pages/Menu/MenuPage.vue"
import NotFoundPage from "./pages/NotFound/NotFoundPage.vue"

const routes = [
    {
        path: '/',
        name: 'menu',
        component: MenuPage
    },
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