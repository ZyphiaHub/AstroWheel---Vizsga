import { goto } from "$app/navigation";

const createSessionStore = () => {

  return {
    setToken: (token) => {
      sessionStorage.setItem('token', token);
    },
    getToken: () => {
      try {
        const token = sessionStorage.getItem('token')
        if (!token) {
          goto("/")
        }
        return token;
      } catch (e) {
        // handle url based search without login
      }

    },
    clearToken: () => {
      sessionStorage.removeItem('token');
    }
  };
};

export const sessionStore = createSessionStore();