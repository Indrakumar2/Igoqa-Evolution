import React, { Component,Fragment } from 'react';
import { HeaderData } from './headerData.js';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData } from '../../../../utils/commonUtils';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import CustomModal from '../../../../common/baseComponents/customModal';
const localConstant = getlocalizeData();
class ChildContracts extends Component {
    constructor(props) {
        super(props);        
        this.state = {
            isOpen: false
        };
    };
    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if (tabInfo && tabInfo.componentRenderCount === 0) {
            this.props.actions.FetchChildContractsOfPrarent(this.props.selectedContractStatus);
        }
    }
    handleChangeStatus = (e) => {
        this.props.actions.FetchChildContractsOfPrarent(e.target.value);
    }

    render() { 
        const headData = HeaderData;
        const rowData = this.props.contractprojectDetail;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
            <div className="customCard mt-0">
                <div className="row mb-0">
                    <div className="col s2 " >
                        <p className="bold">{localConstant.contract.childContract.CONTRACTS}</p>

                    </div>
                    <div className="offset-s6 col s4">
                        <LabelwithValue
                            className=" col s6 pr-2 mt-2 m6 right-align"
                            label={localConstant.contract.childContract.CONTRACT_STATUS}
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
                            defaultValue= {this.props.selectedContractStatus}
                            disabled={this.props.interactionMode}
                        />
                    </div>
                </div>
                <div className="customCard">
                    <ReactGrid gridRowData={rowData} gridColData={headData} paginationPrefixId={localConstant.paginationPrefixIds.childContract}/>
                </div>
            </div>
        </Fragment>
        );

    }
}

export default ChildContracts;
