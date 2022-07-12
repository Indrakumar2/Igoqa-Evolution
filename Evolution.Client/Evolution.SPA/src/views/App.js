import React, { Component,Fragment } from 'react';
import '../assets/externalLibrary/material-design-iconic-font/css/material-design-iconic-font.min.css';
import { AppMainRoutes,AppDashBoardRoutes } from '../routes/routesConfig'; 
import '../assets/css/index.css';
import authService from '../authService';
import ErrorBoundary from '../components/error/errorBoundary';
import { MainRoutes } from '../routes/mainRoutes'; 

class App extends Component {
  constructor(props) {
    super(props);
    if (window.performance) {
      if(authService.isTokenAvailable()){  // D-55
        const { pathname = "" } = this.props.location;
        if (performance.navigation.type == 1 && pathname.indexOf(AppMainRoutes.dashboard) === -1) {
          this.props.history.push(AppDashBoardRoutes.assignments);
        } 
      }
      else{ // D-55
          this.props.history.push(AppMainRoutes.login);
      }
    }
    //On refresh give the confirmation 
    //D-73
    // window.onbeforeunload=()=>{
    //   if(authService.componentHasUnsavedChanges()){
    //     return localConstant.validationMessage.UNSAVED_DATA_VALIDATION;
    //   }
    // };
    // window.onbeforeunload = authService.componentHasUnsavedChanges()? true : undefined;

    // this.props.history.listen((location, action) => {
    //  
    //   if(location.pathname=== AppMainRoutes.company){
    //     const confirmationObject = {
    //       title: "confirmation...",
    //       message: "Are you sure you want to exit",
    //       type: "confirm",
    //       modalClassName: "warningToast",
    //       buttons: [
    //         {
    //           buttonName: "Yes",
    //           onClickHandler: this.deleteSelectedPayroll,
    //           className: "modal-close m-1 btn-small"
    //         },
    //         {
    //           buttonName: "No",
    //           onClickHandler: this.confirmationRejectHandler,
    //           className: "modal-close m-1 btn-small"
    //         }
    //       ]
    //     };
    //     this.props.actions.DisplayModal(confirmationObject);
    //   }
    //   //check if refresh token expires redirect to login page
    //   if(authService.isRefreshTokenExpired()){

    //     // this.props.actions.handleLogOut(); // Commented for D-55

    //      //To avoid looping of check, implemented if condition to check the route is in login.
    //     if(this.props.history.location.pathname!== AppMainRoutes.login){  // D-55
    //       this.props.actions.handleLogOut().then(res=>{
    //         this.props.history.push(AppMainRoutes.login);
    //       });
    //     }
    //   }

    //   //check if access token expires make a call to renew access token
    //   if(authService.isAccessTokenExpired()){
    //     this.props.actions.RefreshToken();
    //   }
    // });
  }
 
  render() {
    // TO-DO: If possible need to change the approach as react ( history.location )
    const uri = window.location.toString();
    if (uri.indexOf("?") > 0) {
      const clean_uri = uri.substring(0, uri.indexOf("?"));
      window.history.replaceState({}, document.title, clean_uri);
      window.scrollTo(0, 0);
    }
    return (<ErrorBoundary>
           <MainRoutes />
         </ErrorBoundary>
    );
  }
}

export default App;
