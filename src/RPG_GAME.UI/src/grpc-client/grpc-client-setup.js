import { BattlePromiseClient } from "./battle_grpc_web_pb";

function getToken() {
    const auth = JSON.parse(window.localStorage.getItem('user-data'));
    return auth ? auth.accessToken : null;
}

/**
 * @constructor
 * @implements {UnaryInterceptor}
 */
  class SimpleUnaryInterceptor {
    constructor() { }
    /** @override */
    intercept(request, invoker) {
        const metadata = request.getMetadata();
        const token = getToken();

        if (token) {
            metadata['Authorization'] = `Bearer ${token}`;
        }

        return invoker(request);
    }
}

const clientGrpc = new BattlePromiseClient(window._env_?.VUE_APP_BACKEND_GRPC_URL ? window._env_.VUE_APP_BACKEND_GRPC_URL : process.env.VUE_APP_BACKEND_GRPC_URL, null, 
    {'unaryInterceptors': [new SimpleUnaryInterceptor()]});

export default clientGrpc;
