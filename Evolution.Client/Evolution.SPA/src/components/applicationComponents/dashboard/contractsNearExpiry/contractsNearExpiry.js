import React,{ Component,Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { headerData } from './headerData.js';
import PropTypes from 'proptypes';
import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

class ContractsNearExpiry extends Component{
    componentDidMount() {
        this.props.actions.FetchContractsNearExpiry();
    }
    render(){   
        const headData = headerData;
        return(
            <ReactGrid gridRowData={this.props.contractsNearExpiry} gridColData={headData} 
            paginationPrefixId={localConstant.paginationPrefixIds.dashboardContractsNearExpiryGridId}
            />
        );
    }
}

ContractsNearExpiry.prototypes = {
    headData:PropTypes.array.isrequired,
    contractsNearExpiry:PropTypes.array.isrequired
};
ContractsNearExpiry.defaultprops ={
    headData:[],
    contractsNearExpiry:null
};
export default ContractsNearExpiry;