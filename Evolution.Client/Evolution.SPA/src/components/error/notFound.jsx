import React,{ Component } from 'react';
import { isEmptyReturnDefault } from '../../utils/commonUtils';

class NotFound extends Component{
    render(){ 
        const { message, file, line, column, errorStack } = isEmptyReturnDefault(this.props.error,'object');
        // console.log(this.props.error);
        return(
            <div id="notfound">
            <div className="notfound">
                <div className="notfound-404">
                    <h1>404</h1>
                </div>
                <h2>Oops! This Page Could Not Be Found</h2>
                <p>Connection to API server interrupted, this could be due to a problem in Network or server. Please wait for a while and try again. If this problem continues, contact your system admistrator</p>
                <a href="/Login">Move to Login Page</a>

                {/* Temp Code Snippet for Development purpose */}
                { this.props.error && 
                <div className="errorDetail">
                    <span> { `message : ${ message }` } </span>
                    <span> { `file : ${ file }` } </span>
                    <span> { `line : ${ line }` } </span>
                    <span> { `column : ${ column }` } </span>
                    <span> { `errorObject : ${ errorStack }` } </span> 
                </div> }
            </div>
        </div>
        );
    }
}

export default NotFound;