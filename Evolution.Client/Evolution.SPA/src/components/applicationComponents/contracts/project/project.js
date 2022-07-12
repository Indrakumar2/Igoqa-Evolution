import React, { Fragment, Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData } from '../../../../utils/commonUtils';
import CustomModal from '../../../../common/baseComponents/customModal';
const localConstant = getlocalizeData();

class Project extends Component {
    constructor(props){
        super(props);
        this.state = {
            isOpen: false
        };
    }
    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if (tabInfo && tabInfo.componentRenderCount === 0) {
            this.props.actions.FetchContractProjects(this.props.selectedProjectStatusValue);
        }
    }
    handleChangeStatus = (e) => {
        this.props.actions.FetchContractProjects(e.target.value);
    }

    projectRefreshHandler = () => {
        this.props.actions.FetchContractProjects(this.props.selectedProjectStatusValue);
    };

    render() {
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const { contractprojectDetail } = this.props;
        return (
            <Fragment>
                  <CustomModal modalData={modelData} />
                <div className="genralDetailContainer customCard mt-0">
                    <div className="row mb-0">
                        <div className="col s2 " >
                            <p className="bold">{localConstant.contract.Projects.PROJECTS}</p>
                        </div>
                        <div className="offset-s6 col s4">
                        <LabelwithValue
                            className=" col s6 pr-2 mt-2 m6 right-align"
                            label={localConstant.contract.Projects.PROJECT_STATUS}
                        />
                            <CustomInput
                                hasLabel={false}
                                divClassName='col s4 pl-0 m6'
                                type='select'
                                className="browser-default"
                                optionsList={localConstant.commonConstants.statusAll}
                                optionName='name'
                                optionValue="value"
                                onSelectChange={this.handleChangeStatus}
                                defaultValue={this.props.selectedProjectStatusValue}
                                // disabled={this.props.interactionMode} //Commented for Defect 864
                            />
                        </div>
                    </div>
                    <div className="customCard ">
                        <ReactGrid 
                            gridColData={HeaderData} 
                            gridRowData={contractprojectDetail}  
                            handleChangeStatus={this.handleChangeStatus}
                            paginationPrefixId={localConstant.paginationPrefixIds.contractProjects}
                            gridRefreshHandler = { this.projectRefreshHandler } />
                    </div>
                </div >
            </Fragment >
        );
    }
}
export default Project;