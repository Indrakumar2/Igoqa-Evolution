import MaterializeComponent from 'materialize-css';
import store from '../../../store/reduxStore';

export const IntertekToasterRemoveAll = () =>{
    const myNode = document.getElementById("toast-container");
        if(myNode)
             myNode.innerHTML = '';
};

export const IntertekToaster = (htmlData, toastClass) => {
    for (const el of document.getElementsByClassName(toastClass)) {
        // el.remove();
        el.style.display="none";
    }
    const state = store.getState();
    const serverError= state.loginReducer.serverError;
    !serverError && serverError !== null && MaterializeComponent.toast({
        html: htmlData,
        classes: toastClass
    });
};