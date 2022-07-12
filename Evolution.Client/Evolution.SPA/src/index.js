import React from 'react';
import ReactDOM from 'react-dom';
import "@babel/polyfill";
import App from './views/';
import registerServiceWorker from './registerServiceWorker';
import { Provider } from 'react-redux';
import store, { persistor } from './store/reduxStore';
import { BrowserRouter } from 'react-router-dom';
import dotenv from 'dotenv';
import { PersistGate } from 'redux-persist/lib/integration/react';
import TextLoader from './common/baseComponents/textLoader';

dotenv.config();

ReactDOM.render(
    <Provider store={store}>
        {/* the loading and persistor props are both required! */}
        <PersistGate loading={<TextLoader />} persistor={persistor}>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </PersistGate>
        {/* <App /> */}
    </Provider>,
    document.getElementById('root'));
registerServiceWorker();
