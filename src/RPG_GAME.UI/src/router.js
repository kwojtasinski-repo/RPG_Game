import { createRouter, createWebHistory } from "vue-router"
import authService from "./services/AuthService.js"
import MenuPage from "./pages/Menu/MenuPage.vue"
import NotFoundPage from "./pages/NotFound/NotFoundPage.vue"
import ForbiddenPage from "./pages/Forbidden/ForbiddenPage.vue"
import SignInPage from "./pages/Account/SignInPage"
import SignUpPage from "./pages/Account/SignUpPage"
import MapsPage from "./pages/Maps/MapsPage.vue"
import AddMapPage from "./pages/Maps/AddMapPage.vue"
import EditMapPage from "./pages/Maps/EditMapPage.vue"
import ViewMapPage from "./pages/Maps/ViewMapPage.vue"
import CharactersPage from "./pages/Characters/CharactersPage.vue"
import HeroesPage from "./pages/Heroes/HeroesPage.vue"
import AddHeroPage from "./pages/Heroes/AddHeroPage.vue"
import EditHeroPage from "./pages/Heroes/EditHeroPage.vue"
import ViewHeroPage from "./pages/Heroes/ViewHeroPage.vue"
import EnemiesPage from "./pages/Enemies/EnemiesPage.vue"
import AddEnemyPage from "./pages/Enemies/AddEnemyPage.vue"
import EditEnemyPage from "./pages/Enemies/EditEnemyPage.vue"
import ViewEnemyPage from "./pages/Enemies/ViewEnemyPage.vue"
import BattleStartPage from "./pages/Battles/BattleStartPage.vue"
import StartFightPage from "./pages/Fights/StartFightPage.vue"
import ProfilePage from "./pages/Profiles/ProfilePage.vue"
import CreateProfilePage from "./pages/Profiles/CreateProfilePage.vue"
import EditProfilePage from "./pages/Profiles/EditProfilePage.vue"
import BattleHistoryPage from "./pages/Battles/BattleHistoryPage.vue"
import BattlePage from "./pages/Battles/BattlePage.vue"

const routes = [
    // --- menu ---
    {
        path: '/',
        name: 'menu',
        component: MenuPage
    },
    // --- heroes ---
    {
        path: '/maps',
        name: 'all-maps',
        component: MapsPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/maps/add',
        name: 'add-map',
        component: AddMapPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/maps/edit/:mapId',
        name: 'edit-map',
        component: EditMapPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/maps/view/:mapId',
        name: 'view-map',
        component: ViewMapPage,
        meta: {
            auth: true
        }
    },
    // --- characters ---
    {
        path: '/characters',
        name: 'characters',
        component: CharactersPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    // --- heroes ---
    {
        path: '/heroes',
        name: 'all-heroes',
        component: HeroesPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/heroes/add',
        name: 'add-hero',
        component: AddHeroPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/heroes/edit/:heroId',
        name: 'edit-hero',
        component: EditHeroPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/heroes/view/:heroId',
        name: 'view-hero',
        component: ViewHeroPage,
        meta: {
            auth: true
        }
    },
    // --- enemies ---
    {
        path: '/enemies',
        name: 'all-enemies',
        component: EnemiesPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/enemies/add',
        name: 'add-enemy',
        component: AddEnemyPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/enemies/edit/:enemyId',
        name: 'edit-enemy',
        component: EditEnemyPage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    {
        path: '/enemies/view/:enemyId',
        name: 'view-enemy',
        component: ViewEnemyPage,
        meta: {
            auth: true
        }
    },
    // --- login page ---
    {
        path: '/login',
        name: 'login',
        component: SignInPage
    },
    // --- register page ---
    {
        path: '/register',
        name: 'register',
        component: SignUpPage
    },
    // --- profile ---
    {
        path: '/profile',
        name: 'profile',
        component: ProfilePage,
        meta: {
            auth: true
        }
    },
    {
        path: '/profile/create',
        name: 'create-profile',
        component: CreateProfilePage,
        meta: {
            auth: true
        }
    },
    // --- player page (admin) ---
    {
        path: '/profile/edit/:playerId',
        name: 'edit-profile',
        component: EditProfilePage,
        meta: {
            auth: true,
            role: 'admin'
        }
    },
    // --- fight pages ---
    {
        path: '/battles',
        name: 'battle-page',
        component: BattlePage,
        meta: {
            auth: true
        }
    },
    {
        path: '/fights/start',
        name: 'fight-start',
        component: StartFightPage,
        meta: {
            auth: true
        }
    },
    {
        path: '/battles/start',
        name: 'battle-start',
        component: BattleStartPage,
        meta: {
            auth: true
        }
    },
    {
        path: '/histories',
        name: 'battle-histories',
        component: BattleHistoryPage,
        meta: {
            auth: true
        }
    },
    // --- forbidden found page ---
    {
        path: '/forbidden',
        name: 'forbidden',
        component: ForbiddenPage,
        meta: {
            auth: true
        }
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

router.beforeEach(async (to, from, next) => {
    const userLoggedIn = await authService.isLogged();
    if (to.meta.auth && !userLoggedIn) {
      next('/login');
    }

    const user = authService.getUser();
    
    if (to.meta.role && to.meta.role != user.role) {
        next('/forbidden');
    }

    next();
});

export default router;
