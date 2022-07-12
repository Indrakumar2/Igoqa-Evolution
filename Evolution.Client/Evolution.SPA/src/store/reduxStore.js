import { createStore, applyMiddleware, compose } from 'redux';
import thunk from 'redux-thunk';
import { rootReducer }  from '../rootReducer';
import { persistStore } from 'redux-persist';

const middleware = [ thunk ];
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const store = createStore(
    rootReducer,
    composeEnhancers(applyMiddleware(...middleware))
);
export const persistor = persistStore(store);
export default store;