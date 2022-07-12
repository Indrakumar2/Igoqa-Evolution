import React,{ Component,Fragment } from 'react';
class TextLoader extends Component{ 
    render(){         
    
        return(
            <div class="preloader-css">
            <p>Loading</p>           
            <span></span>
            <span></span>
            <span></span>
            <span></span>
          </div>
        );
    }
}

export default TextLoader;