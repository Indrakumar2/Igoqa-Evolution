import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import { getlocalizeData } from '../../../../utils/commonUtils';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import CustomModal from '../../../../common/baseComponents/customModal';
const localConstant = getlocalizeData();

class Assignments extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isOpen: false
        };
    }
    handleChangeStatus = (e) => {
        this.props.actions.FetchAssignments(e.target.value);
    }
    componentDidMount = () => {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if (tabInfo && tabInfo.componentRenderCount === 0)
            this.props.actions.FetchAssignments("No");
    }

    assignmentRefreshHandler = () => {
        this.props.actions.FetchAssignments(this.props.selectedAssignmentStatus);
    };

    render() {
        this.props.assignmentData && this.props.assignmentData.map(assignment => {
            const technicalSpecialists = [];
            if (Array.isArray(assignment.techSpecialists) && assignment.techSpecialists.length > 0) {
                assignment.techSpecialists.map((iterated) => {
                    if(iterated.fullName.trim()!=='')
                    technicalSpecialists.push(iterated.fullName);
                });
                assignment.techSpecialists = (technicalSpecialists).join(',');
            }
        });
        const StatusOptions = [
            { optionName: localConstant.contract.ALL, value: 'all' },
            { optionName: localConstant.commonConstants.YES, value: 'Yes' },
            { optionName: localConstant.commonConstants.NO, value: 'No' }
        ];
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="genralDetailContainer customCard mt-0">
                    <div className="row mb-0">
                        <div className="col s2 " >
                            <p className="bold">{localConstant.project.assignments.ASSIGNMENTS}</p>

                        </div>
                        <div className="offset-s6 col s4">
                            <LabelwithValue
                                className=" col s6 pr-2 mt-2 m6 right-align"
                                label={localConstant.companyDetails.Contracts.COMPLETE_STATUS}
                            />
                            <CustomInput
                                hasLabel={false}
                                divClassName='col s4 pl-0 m6'
                                type='select'
                                className="browser-default"
                                optionsList={StatusOptions}
                                optionName='optionName'
                                optionValue="value"
                                // disabled={this.props.interactionMode} //Commented for Defect 864
                                onSelectChange={this.handleChangeStatus}
                                defaultValue={this.props.selectedAssignmentStatus}
                            />
                        </div>
                    </div>
                    <div className="customCard">
                        <ReactGrid 
                            gridRowData={this.props.assignmentData} 
                            gridColData={HeaderData}
                            paginationPrefixId={localConstant.paginationPrefixIds.projectAssignments}
                            gridRefreshHandler = { this.assignmentRefreshHandler } />
                    </div>
                </div>
            </Fragment>
        );
    }
}

export default Assignments;