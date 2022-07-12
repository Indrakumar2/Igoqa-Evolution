import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './companyDocumentsHeader';
import { bindAction } from '../../../../utils/commonUtils';
import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
class CompanyDocuments extends Component {
    constructor(props) {
        super(props);
        this.state = {          
        };
        this.updatedData = {};
        //bindAction(HeaderData, "reassign", this.editRowHandler);
    }
        
    componentDidMount(){  
       
    }
    editRowHandler = (data) => {
              
    }
    render() {
        const { CompanyDocumentsData } = this.props;
        return (                                 
			<div className="customCard">
            <h6 className="bold"> <span> {localConstant.techSpec.common.INTERTEK_COMPANY_DOCUMENTS} </span></h6>
               <ReactGrid gridRowData={CompanyDocumentsData} gridColData={HeaderData} />    
            </div>
                  
        );
    }
}

export default CompanyDocuments;
