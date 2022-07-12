import React,{ Component } from 'react';

class AuthenticationFailed extends Component{
    render(){ 
        return(
            <div id="notfound">
            <div className="notfound">
                <div className="notfound-404">
                    <h1>401</h1>
                </div>
                <p>Authentication Failed</p>
                <a href="/Login">Move to Login Page</a>
            </div>
        </div>
        );
    }
}

export default AuthenticationFailed;