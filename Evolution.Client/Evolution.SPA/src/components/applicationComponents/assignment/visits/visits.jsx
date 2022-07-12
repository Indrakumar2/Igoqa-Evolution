import React, { Component ,Fragment } from 'react';
import { HeaderData } from './headerData.js';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData } from '../../../../utils/commonUtils';
import CustomModal from '../../../../common/baseComponents/customModal';
//import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
//import CustomInput from '../../../../common/baseComponents/inputControlls';
import PropTypes from 'prop-types';
const localConstant = getlocalizeData();

class Visits extends Component {
    componentDidMount() {
          const tabInfo = this.props.tabInfo;
          /** 
           * Below check is used to avoid duplicate api call
           * the value to isTabRendered is set in customTabs on tabSelect event handler
          */
          if (tabInfo && tabInfo.componentRenderCount === 0) {
            this.props.actions.FetchAssignmentVisits();
          }
    }
    RefereshVisits=()=>
    {
        this.props.actions.FetchAssignmentVisits();

    }

    // handleChangeStatus = (e) => {
    //     this.props.actions.FetchAssignmentVisits(e.target.value);
    // }
    render() {
        const headData = HeaderData.VisitHeader;
        const rowData = this.props.visitData;
        return (
            <Fragment>
            <CustomModal />
            <div className="customCard mt-0">
                <div className="row mb-0">
                    <div className="col s2 " >
                        <p className="bold">{localConstant.visit.VISITS}</p>

                    </div>
                    {/* <div className="offset-s6 col s4">
                        <LabelwithValue
                            className=" col s6 pr-2 mt-2 m6 right-align"
                            label={localConstant.visit.VISIT_STATUS}
                        />
                        <CustomInput
                            hasLabel={false}
                            divClassName='col s4 pl-0 m6'
                            type='select'
                            className="browser-default"
                            optionsList={localConstant.commonConstants.visitStatus}
                            optionName='name'
                            optionValue="value"
                            onSelectChange={this.handleChangeStatus}
                            defaultValue={this.props.selectedVisitStatus}
                        />
                    </div> */}
                </div>
                {/* <a className="waves-effect btn-small right invoiceNotesMargin" onClick={this.RefereshHistoricalVisits}>
                    {localConstant.commonConstants.REFRESH} </a> */}
                   
                <div className="customCard">
                <a  onClick={this.RefereshVisits} className="right mt-3 mr-2 btn-small bold"><i className="zmdi zmdi-refresh zmdi-hc-lg"></i></a>
                    <ReactGrid 
                        gridRowData={rowData} 
                        gridColData={headData}
                        paginationPrefixId={localConstant.paginationPrefixIds.assignmentTIS} />
                </div>
            </div>
        </Fragment>
        );
    }
}
export default Visits;
Visits.propTypes = {
    visitData: PropTypes.array,
    selectedVisitStatus:PropTypes.string
};

Visits.defaultProps = {
    visitData: [],
    selectedVisitStatus:'O'
};