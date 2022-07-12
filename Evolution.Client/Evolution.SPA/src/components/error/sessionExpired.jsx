import React,{ Component } from 'react';

class SessionExpired extends Component{
    render(){ 
        return(
            <div id="notfound">
            <div className="session">
                <div className="sessionExpired">
                    <h1>Session Expired</h1>
                </div>
                <p>Your Session has been Expired. Please login again.</p>
                <a href="/Login">Move to Login Page</a>
            </div>
        </div>
        );
    }
}

export default SessionExpired;