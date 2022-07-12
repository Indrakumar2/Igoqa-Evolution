import React, { Component,Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './invoiceHeaders';
import { getlocalizeData, formInputChangeHandler,isEmpty } from '../../../../utils/commonUtils';

class Invoice extends Component {
    constructor(props) {
        super(props);
        this.state = {          
          
        };
        this.updatedData = {};
    }
  
    render() {         
        return ( 
                <div className="customCard">
                <ReactGrid gridRowData={this.props.InvoiceData} gridColData={HeaderData} />                
                </div>
        );
    }
}

export default Invoice;
