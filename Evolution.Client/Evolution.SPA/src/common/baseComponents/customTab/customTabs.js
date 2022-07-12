import React, { Component, Fragment } from 'react';
import PropTypes from 'prop-types';
import { isEmpty, SetCurrentModuleTabInfo,isFunction } from '../../../utils/commonUtils';
import { GetComponent } from '../../generateFields/findComponent';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';

class CustomTabs extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isDetailPanelOpen: false,
        };
    }
    toggleDetailSlide=()=>{        
        this.setState({ isDetailPanelOpen:!this.state.isDetailPanelOpen });
       }
    //Tabs Header Generating 
    tabsHeaderPanel = (tabsList,currentPage) => {
        return !isEmpty(tabsList) ? tabsList.map((tabs, index) => (
            tabs.tabActive ?
                tabs.tabDisableStatus.length > 0 ?
                    tabs.tabDisableStatus.map((tabDisable, index) => (
                        (tabDisable === currentPage ? null : <Tab key={tabs.tabBody} tabuniqueid={tabs.tabBody} > {tabs.tabHeader} </Tab>)
                    )
                    )
                    : <Tab key={tabs.tabBody}  tabuniqueid={tabs.tabBody}> {tabs.tabHeader} </Tab>
                : null
        )):null;
    }
    //Tabs Boday Generating 
    tabsBodyPanel = (tabsList, moduleName, currentPage,interactionMode,callBackFuncs,editMode,editViewMode,techManager) => {
        return !isEmpty(tabsList) ? tabsList.map((block, index) => {
            const Component = GetComponent(block.tabBody, moduleName);
            return Component !== null ?
                (block.tabActive ?
                    (block.tabDisableStatus.length > 0 ?
                        block.tabDisableStatus.map((tabDisable, index) => {
                            return (tabDisable === currentPage ? null :
                                <TabPanel className={this.state.isDetailPanelOpen ? "fixedCard tab-panel-Open " :"fixedCard tab-panel-Close"} key={index}> {<Component interactionMode={interactionMode} currentPage={currentPage}  callBackFuncs={callBackFuncs} editMode={editMode} techManager={techManager} tabInfo = {block}/>} </TabPanel>
                            );
                        })
                        : <TabPanel className={this.state.isDetailPanelOpen ? "tab-panel-Open" :"tab-panel-Close"} key={index}> {<Component interactionMode={interactionMode} currentPage={currentPage} callBackFuncs={callBackFuncs} editMode={editMode} editViewMode={editViewMode} techManager={techManager}  tabInfo = {block}/>} </TabPanel>)
                    : null) : null;
        }):null;
    }

  tabSelect = (index, lastIndex, event) => {
    const tabId = event.target.getAttribute("tabuniqueid");
    /**
     * SetCurrentModuleTabInfo Method will set currentTab and isTabRendered(bool params)
     * isTabRendered is used to avoid unnecessary api calls in each tab of respective module
     */
    const tabsList  = this.props.tabsList;
    SetCurrentModuleTabInfo(tabsList, tabId);
    if(isFunction(this.props.onSelect)){
        this.props.onSelect();
    }
  }
    render() {    

        const { tabsList,moduleName, currentPage, interactionMode,callBackFuncs,editMode,editViewMode,techManager } = this.props;
        return (
            <Tabs onSelect={this.tabSelect}>
               <span className={this.state.isDetailPanelOpen ? "toggleDetailSliderOpen" :"toggleDetailSliderClose"} onClick={this.toggleDetailSlide}><i className={this.state.isDetailPanelOpen ? "zmdi zmdi-chevron-left" :"zmdi zmdi-chevron-right" }></i></span>
                <TabList className={this.state.isDetailPanelOpen ? "react-tabs__tab-list tab-list-open" :"react-tabs__tab-list tab-list-Close"}>
                  {this.tabsHeaderPanel(tabsList, currentPage)}
                </TabList>
                {this.tabsBodyPanel(tabsList, moduleName, currentPage, interactionMode,callBackFuncs,editMode,editViewMode,techManager)}
            </Tabs>
        );
    }
}

export default CustomTabs;
CustomTabs.propTypes = {
    rootModule: PropTypes.string    
};
CustomTabs.propTypes = {
    rootModule:'Evo2'   
};