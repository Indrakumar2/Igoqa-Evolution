import React,{ Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { headerData } from './headerData';
import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
class Documentation extends Component{
   componentDidMount(){
       this.props.actions.DashboardDocumentationDetails();
   }
    render(){   
        
        return(

                  <ReactGrid gridRowData={this.props.documentationDetails}  gridColData={headerData} 
                  paginationPrefixId={localConstant.paginationPrefixIds.dashboardDocumentationGridId}
                  />
        //    </div>
           
        );
    }
}

export default Documentation;