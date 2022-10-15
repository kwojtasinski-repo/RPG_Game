import { mount } from '@vue/test-utils'
import MenuPage from '@/pages/Menu/MenuPage.vue'

describe('MenuPage.vue', () => {
    const store = {
        getters: {
            user: null
        }
    }

    it('should render', () => {
        const wrapper = mount(MenuPage, { global: { mocks: { $store: store } } });
        expect(wrapper).toBeTruthy();
    })

    it('given not authenticated user should show menu with 2 selections', () => {
        const expectedStrings = ['Login', 'Register'];

        const wrapper = mount(MenuPage, { global: { mocks: { $store: store } } });

        const text = wrapper.text();
        expect(expectedStrings.some(str => text.includes(str))).toBeTruthy();
    })

    it('given authorized user should show menu with 4 selections', () => {
        store.getters.user = { role: 'user' }
        const expectedStrings = ['Start fight', 'Profile', 'History', 'Logout'];

        const wrapper = mount(MenuPage, { global: { mocks: { $store: store } } });

        const text = wrapper.text();
        expect(expectedStrings.some(str => text.includes(str))).toBeTruthy();
    })

    it('given authorized admin should show menu with 6 selections', () => {
        store.getters.user = { role: 'admin' }
        const expectedStrings = ['Start fight', 'Profile', 'History', 'Characters', 'Maps', 'Logout'];

        const wrapper = mount(MenuPage, { global: { mocks: { $store: store } } });

        const text = wrapper.text();
        expect(expectedStrings.some(str => text.includes(str))).toBeTruthy();
    })
})