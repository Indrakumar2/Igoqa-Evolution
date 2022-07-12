import React, { Component,Fragment } from 'react';
import PropTypes from 'prop-types';
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import { isEmptyReturnDefault } from '../../../utils/commonUtils';
class Menu extends Component {
    componentDidMount(){
        const button = document.getElementById('slide');
        button.onclick = function () {
            const container = document.querySelector('.scrollableTabs');
            sideScroll(container,'right',25,100,10);
        };

        const back = document.getElementById('slideBack');
        back.onclick = function () {
            const container = document.querySelector('.scrollableTabs');
            sideScroll(container,'left',25,100,10);
        };

        function sideScroll(element,direction,speed,distance,step){
            let scrollAmount = 0;
            const slideTimer = setInterval(function(){
                if(direction == 'left'){
                    element.scrollLeft -= step;
                } else {
                    element.scrollLeft += step;
                }
                scrollAmount += step;
                if(scrollAmount >= distance){
                    window.clearInterval(slideTimer);
                }
            }, speed);
        }
        //Active Menu Scroll Position Function

    }
    
    componentDidUpdate() {
        const element = document.querySelectorAll('.scrollableTabs');
        const containerWith = element[0].clientWidth;
        const elementChild = document.querySelectorAll('.scrollableTabs li');
        let elementNode;
            for (elementNode of elementChild) {
                if(elementNode.classList.contains('active')){                   
                    if (elementNode.offsetLeft > containerWith) {
                        element.item(0).scrollLeft = (elementNode.offsetLeft - containerWith) + 100;
                    }
                    }
                } 
    }
    getNavStyles = (path) => {
        return this.context.router.route.location.pathname === path ? 'active' : 'inactive';
    }
    render() {
        const { menuClass, menulist,name,url,count,countkey, isScrollable,userType } = this.props;
        const menulistArray = isEmptyReturnDefault(menulist,'array');

        return (
            <Fragment>
                {/* <div className={isScrollable?'tabPrevBtn tabControls show':'hide'}>
                    <a id="slideBack" className="link ml-2"><i className="zmdi zmdi-chevron-left zmdi-hc-2x"></i></a>                
                </div> 
                <ul className={'mb-0 mt-0 collection '+ menuClass }>
                  {
                    menulistArray&& menulistArray.map((menu,i) => {
                               const isDisable = menu.isDisableMenu && menu.isDisableMenu.length >0 ? (menu.isDisableMenu.map((m)=>{
                                    if(userType === m){
                                        return true;
                                    }
                                    return false;
                               }))
                               : false ;
                          return(
                            !isDisable[0] ?  <li key={i} className={"collection-item " + this.getNavStyles(menu[url])}>
                                    <Link key={i} to={menu[url]} >{menu[name]} {count && count[menu[countkey]] !== null &&  count[menu[countkey]] !== undefined && <span>({count[menu[countkey]]})</span>}</Link>
                            </li> : null
                            );                           
                      })
                  }
                    
              </ul>
               <div className={isScrollable?'tabNextBtn tabControls show':'hide'}>
                    <a id="slide" className="link mr-2"><i className="zmdi zmdi-chevron-right zmdi-hc-2x"></i></a>              
               </div> */}

                <div className={'tabControls'}>
                    <a id="slideBack" className={isScrollable ?  'tabPrevBtn link ml-2 show':'hide' }><i className="zmdi zmdi-chevron-left zmdi-hc-2x"></i></a>
                   <ul className={'mb-0 mt-0 collection '+ menuClass }>

                        {
                            menulistArray&& menulistArray.map((menu,i) => {
                                const isDisable = menu.isDisableMenu && menu.isDisableMenu.length >0 ? (menu.isDisableMenu.map((m)=>{
                                    if(userType === m){
                                        return true;
                                    }
                                    return false;
                                }))
                                    : false ;
                                return(
                                    !isDisable[0] ?  <li key={i} className={"collection-item " + this.getNavStyles(menu[url])}>
                                        <Link key={i} to={menu[url]} >{menu[name]} {count && count[menu[countkey]] !== null &&  count[menu[countkey]] !== undefined && <span>({count[menu[countkey]]})</span>}</Link>
                                    </li> : null
                                );
                            })
                        }
                    </ul>
                    <a id="slide" className={isScrollable ?  'tabNextBtn link ml-2 show':'hide' }><i className="zmdi zmdi-chevron-right zmdi-hc-2x"></i></a>

                </div>
            </Fragment>
        );
    }
}
export default Menu;
Menu.propTypes = {
    menuClass: PropTypes.string,
    menulist: PropTypes.array.isRequired
};
Menu.contextTypes= {
    router: PropTypes.object
};
Menu.defaultProps = {
    menuClass: '',
    menulist:[],
    count:''

}; 