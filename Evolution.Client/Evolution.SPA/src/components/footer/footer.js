import React,{ Component } from 'react';
class AppFooter extends Component{
    render(){
        return(
            <footer className = "page-footer" >
                <div className = "col s12 center-align" >
                    <span> Copyright&copy; 2018 Intertek </span>  
                    <a href = "#!"> -About </a> 
                    <a href = "#!" > -Terms </a>
                    <a href = "#!" > -Privacy Policy </a>
                </div> 
            </footer>
        );
    }
}

export default AppFooter;