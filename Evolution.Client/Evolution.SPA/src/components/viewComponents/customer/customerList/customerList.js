import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './customerHeaderData.js';
import { getlocalizeData } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
const localConstant = getlocalizeData();
class CustomerList extends Component {
  constructor(props) {
    super(props);
    this.updatedData = {};
  }

  componentWillUnmount() {
    //this.props.actions.ClearSearchData(); // D-169
  }

  componentDidMount() {
    const custModal = document.querySelectorAll('.modal');
    const custModalInstances = MaterializeComponent.Modal.init(custModal, { "dismissible": false });
    const select = document.querySelectorAll('select');
    const selectInstances = MaterializeComponent.FormSelect.init(select);
    const collapsibleHeader = document.querySelectorAll('.collapsible');
    const collapsibleInstances = MaterializeComponent.Collapsible.init(collapsibleHeader);

    //commented, Now we are getting FetchCountry and customer Documents from master data reducer
    // this.props.actions.FetchCountry();
    // this.props.actions.FetchMasterDocumentTypes();

    // document.getElementById("operatingCountry").addEventListener("keydown", function (e) {
    //   if (e.key === "Enter" || (e.keyCode || e.which) === 13){
    //     e.preventDefault();
    //     document.getElementById("clicked-search").click();
    //   }
    // });
    this.props.actions.ClearSearchData(); // D-169
  }

  handlerChange = (e) => {   
    if(e.target.name === 'parentCompanyName' || e.target.name === 'customerName')
    {
      this.updatedData[e.target.name] = e.target.value.trim().replace(/&/g, "%26");
    } 
    else 
    {
      this.updatedData[e.target.name] = e.target.value.trim();
    }    
  }

  searchFilter = (e) => {
    e.preventDefault();
    this.props.actions.FetchCustomerList(this.updatedData);
    
  }
  ClearSearchData = () => {
    this.updatedData = {};
    this.props.actions.ClearSearchData(this.updatedData); 
  }

  render() {
    const headData = HeaderData;
    const rowData = this.props.customerData;
    const countryMasterData = this.props.countryMasterData;
    const { documentTypesData } = this.props;
  return (
      <Fragment>
        <div className="contentSection">
          <div className="customCard">
            <ul className="collapsible">
              <li className="active">
                <div className="collapsible-header">
                  <strong className="bold">{localConstant.customer.SEARCH_CUSTOMER }</strong><span className="pl-1">{`${ localConstant.customer.EDIT_VIEW_CUSTOMER }`}</span>
                  <div className="searchIcon"><i className="zmdi zmdi-chevron-down collapsibleChevron"></i></div>
                </div>
                <div className="collapsible-body">
                  <form id="searchPanel" onSubmit={this.searchFilter} onReset={this.ClearSearchData}>
                    <div className="row">
                      <div className="col s4 pl-0">
                        <label htmlFor="customerName" className="customLabel">{localConstant.customer.CUSTOMER_NAME}</label>
                        <input className="browser-default customInputs" name="customerName" onInput={this.handlerChange} id="customerName" type="text" autoComplete="off" />
                      </div>                     
                      <div className="col s4 pl-0">
                        <label htmlFor="parentCompanyName" className="customLabel">{localConstant.customer.PARENT_COMPANY_NAME}</label>
                        <input className="browser-default customInputs" name="parentCompanyName" onInput={this.handlerChange} id="parentCompanyName" type="text" autoComplete="off" />
                      </div>
                      <div className="col s4 pl-0">
                        <label htmlFor="customerCode" className="customLabel">{localConstant.customer.CUSTOMER_CODE}</label>
                        <input className="browser-default customInputs" name="customerCode" onInput={this.handlerChange} id="customerCode" type="text" autoComplete="off" />
                      </div>
                    </div>
                    <div className="row">
                      <div className="col s4 pl-0">
                        <label className="customLabel">{localConstant.customer.OPERATING_COUNTRY}</label>
                        <select className="customInputs browser-default" id="operatingCountry" name="operatingCountry" onChange={this.handlerChange}>
                          <option value="">Select</option>
                          {countryMasterData ?
                            countryMasterData.map((data, i) =>
                              <option key={i} value={data.name}>{data.name}</option>) :
                            (IntertekToaster('countryMasterData api is down..., Try after sometime', 'warningToast OperatingContErrToast'))
                          }
                        </select>
                      </div>
                      <div className="col s3   pl-0">
                        <label className="customLabel"> Search Documents</label>
                        <select className="browser-default customInputs" name="searchDocumentType" onChange={this.handlerChange} >
                        <option value="">Select</option>
                          {documentTypesData ?
                            (documentTypesData.map((data, i) =>
                            <option key={i} value={data.name}>{data.name}</option>))                            
                             :null
                          };
                        </select>
                      </div>
                      <div className=" col s3 pl-0">
                        <label htmlFor="hasTheWord" className="customLabel">Search Document(s) For Words</label>
                        <input className="browser-default customInputs" name="documentSearchText" onInput={this.handlerChange} id="hasTheWord" type="text" maxLength={500} />
                      </div>
                      <div className="mt-4x col s2  pl-0">
                        <button type="submit" id="clicked-search" className="modal-close waves-effect waves-green btn">Search</button>
                        <button type="reset" className="modal-close waves-effect waves-green btn ml-2">Reset</button>
                      </div>

                    </div>
                  </form>
                </div>
              </li>
            </ul>
            <ReactGrid gridCustomClass={'preWarp'} gridRowData={rowData} gridColData={headData} />
          </div>
        </div>
      </Fragment>
    );
  }
}

export default CustomerList;
