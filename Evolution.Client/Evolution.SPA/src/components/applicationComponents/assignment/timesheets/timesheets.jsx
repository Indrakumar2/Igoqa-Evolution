import React, { Component, Fragment } from 'react'; 
import { HeaderData } from './headerData';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData } from '../../../../utils/commonUtils';
// import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
// import CustomInput from '../../../../common/baseComponents/inputControlls';
import CustomModal from '../../../../common/baseComponents/customModal';
import PropTypes from 'prop-types';
const localConstant = getlocalizeData();
class TimeSheets extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isOpen: false
        };
    }
    // handleChangeStatus = (e) => {
    //     this.props.actions.FetchAssignmentTimesheets(e.target.value);
    // }
    componentDidMount = () => {
        const tabInfo = this.props.tabInfo;
          /** 
           * Below check is used to avoid duplicate api call
           * the value to isTabRendered is set in customTabs on tabSelect event handler
          */
          if (tabInfo && tabInfo.componentRenderCount === 0) {
            this.props.actions.FetchAssignmentTimesheets();
          }
    }

    timesheetRefreshHandler = () => {
        this.props.actions.FetchAssignmentTimesheets();
    };

    render() {    
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="genralDetailContainer customCard mt-0">
                    <div className="row mb-0">
                        <div className="col s2 " >
                            <p className="bold">{localConstant.timesheet.TIMESHEETS}</p>

                        </div>
                        {/* <div className="offset-s6 col s4">
                            <LabelwithValue
                                className=" col s6 pr-2 mt-2 m6 right-align"
                                label={localConstant.timesheet.TIMESHEET_STATUS}
                            />
                            <CustomInput
                                hasLabel={false}
                                divClassName='col s4 pl-0 m6'
                                type='select'
                                className="browser-default"
                                optionsList={localConstant.commonConstants.timesheetStatus}
                                optionName='name'
                                optionValue="value"
                                onSelectChange={this.handleChangeStatus}
                                defaultValue={this.props.selectedTimesheetStatus}
                            />
                        </div> */}
                    </div>
                    <div className="customCard">
                        <ReactGrid 
                            gridRowData={this.props.timesheetData} 
                            gridColData={HeaderData}
                            gridRefreshHandler = { this.timesheetRefreshHandler }
                            paginationPrefixId={localConstant.paginationPrefixIds.assignmentTSS} />
                    </div>
                </div>
            </Fragment>
        );
    }
}

export default TimeSheets;
TimeSheets.propTypes = {
    timesheetData: PropTypes.array,
    selectedTimesheetStatus:PropTypes.string
};

TimeSheets.defaultProps = {
    timesheetData: [],
    selectedTimesheetStatus:'C'
};