import React,{ Component } from 'react';

class NotFound extends Component{
    render(){ 
        return(
            <div id="notfound">
            <div className="notfound">
                <div className="notfound-404">
                    <h1>500</h1>
                </div>
                <p>Connection to API server interrupted, this could be due to a problem in Network or server. Please wait for a while and try again. If this problem continues, contact your system admistrator</p>
                <a href="/Login">Move to Login Page</a>
            </div>
        </div>
        );
    }
}

export default NotFound;