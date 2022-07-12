import React from 'react';
import NotFound from './notFound';
import InternalServer from './internalServer';
import SessionExpired from './sessionExpired';
import AuthenticationFailed from './authenticationFailed';
import { AppMainRoutes } from '../../routes/routesConfig';
import { withRouter } from 'react-router-dom';
import { IntertekToasterRemoveAll } from '../../common/baseComponents/intertekToaster';

class ErrorBoundary extends React.Component {
    constructor(props) {
        super(props);
        const { location } = this.props.history;
        this.state = { 
            hasError: false,
            status: location.state&& location.state.status?location.state.status:this.props.status
        };

    }

    componentWillMount() {
        this.startErrorLog(this.props);
    }

    startErrorLog(props) {
        window.onerror = (message, file, line, column, errorObject) => {
            column = column || (window.event && window.event.errorCharacter);
            const stack = errorObject ? errorObject.stack : null;

            //trying to get stack from IE
            if (!stack) {
                const stack = [];
                let f = arguments.callee.caller;
                while (f) {
                    stack.push(f.name);
                    f = f.caller;
                }
                errorObject['stack'] = stack;
            }

            const data = {
                message: message,
                file: file,
                line: line,
                column: column,
                errorStack: stack
            };

            // Temp Code Snippet for Development purpose
            this.setState({ error: data });

            //here I make a call to the server to log the error

            //the error can still be triggered as usual, we just wanted to know what's happening on the client side
           
            //return false;
        };
    }

    componentDidCatch(error, info) {
        // console.log("ErrorBoundary - componentDidCatch");
        console.log(error);
        // console.log(info);
        // Display fallback UI
        this.setState({ hasError: true });
        // You can also log the error to an error reporting service
        this.startErrorLog(error, info);
    }

    render() {
        IntertekToasterRemoveAll();
		//console.log("this.state.status = ",this.state.status, "this.state.hasError = ", this.state.hasError,"this.props.history.location.pathname = ",this.props.history.location.pathname );
        if(this.state.status === 404 || this.state.hasError){
            // console.log("ErrorBoundary (this.state.status === 404 || this.state.hasError)");
            // console.log("this.state.hasError :" + this.state.hasError);
            return <NotFound error = { this.state.error } />;
        }
        if(this.state.status === 500){
            // console.log("ErrorBoundary (this.state.status === 500)");
            return <InternalServer />;
        }
        if(this.state.status === 401){
            // console.log("ErrorBoundary this.state.status === 401");
            return <AuthenticationFailed />;
        }
        if(this.state.status === 200 && this.props.history.location.pathname === AppMainRoutes.error){
            // console.log("this.state.status === 200 && this.props.history.location.pathname === AppMainRoutes.error)");
            return <SessionExpired />;
        }
        return this.props.children;
    }

}

export default withRouter(ErrorBoundary);