import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './mySearchHeader';
import { bindAction,getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();

class MySearch extends Component {
    constructor(props) {
        super(props);
        this.state = {          
        };
        this.updatedData = {};
        bindAction(HeaderData, "reassign", this.editRowHandler);
    }
        
    componentDidMount(){  
       //this.props.actions.grmSearchMatsterData();   
        this.props.actions.FetchMySearchData();
    }
    editRowHandler = (data) => {
              
    }
    render() {
        const { mySearchData } = this.props;
        return (                                 
			<ReactGrid gridRowData={mySearchData} gridColData={HeaderData} 
            paginationPrefixId={localConstant.paginationPrefixIds.dashboardMySearchGridId}/>          
        );
    }
}

export default MySearch;
