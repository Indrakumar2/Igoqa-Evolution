import React,{ Component } from 'react';

class Forbidden extends Component{
    render(){ 
        return( 
            <div id="forbidden">
            <div className="forbidden">
                <div className="forbidden-403">
                <h1>403</h1>
                </div>
                <h2>FORBIDDEN!</h2>
                <p>You don't have permission to access the requested page </p>               
            </div>
        </div>
        );
    }
}

export default Forbidden;