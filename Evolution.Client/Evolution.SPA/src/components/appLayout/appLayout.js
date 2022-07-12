import React, { Component, Fragment } from 'react';
import AppHeader from '../header';
import AppSideMenu from '../sideMenu';
import Title from '../../common/baseComponents/pageTitle';
import LoaderComponent from '../../common/baseComponents/loader';

class AppLayout extends Component {
  constructor(props){
    super(props);
    this.state={
      isOpen:true
    };
    window.addEventListener("storage", this.sessionStorage_transfer, false);
  };

  handleClick = (e)=>{
    e.preventDefault();
    this.setState({ isOpen:!this.state.isOpen });
  };

  sessionStorage_transfer = function(event) {
    if(!event) { event = window.event; }
    if(!event.newValue) return;          // do nothing if no value to work with
    if (event.key == 'getSessionStorage') {
        // another tab asked for the sessionStorage
        localStorage.setItem('sessionStorage', JSON.stringify(sessionStorage));
        // the other tab should now have it.
        localStorage.removeItem('sessionStorage');
    } else if (event.key == 'sessionStorage' && !sessionStorage.length) {
        // another tab sent data
        const data = JSON.parse(event.newValue);
        for (const key in data) {
            sessionStorage.setItem(key, data[key]);
        }
    }
  };

  componentDidMount() {
    if (!sessionStorage.length) {
      localStorage.setItem('getSessionStorage', 'sessionStorage');
      localStorage.removeItem('getSessionStorage', 'sessionStorage');
    };
  }

  render() {
    return (
      <Fragment>
      { this.props.loader && <LoaderComponent isShowLoader={this.props.loader}></LoaderComponent>}

        <div className="wrapper">
          <AppSideMenu isSideBarStatus={this.state.isOpen}/> 
          <Title Title="Evolution" />         
          <AppHeader isSideBarStatus={this.state.isOpen} name="Customer Lifecycle Management" panelClick={this.handleClick}></AppHeader>             
                  
            <div className={this.state.isOpen ? 'container-fluid main-panel' :'container-fluid main-panel haff-wdith'}>
          
               {this.props.children} 
            </div>                
          </div>     
      </Fragment>
    );
  }
}

export default AppLayout;
