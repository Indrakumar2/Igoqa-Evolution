import React, { Component, Fragment } from 'react';
import Title from '../../../../common/baseComponents/pageTitle';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import HeaderData from '../companyHeaderData';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Panel from '../../../../common/baseComponents/panel';
import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant=getlocalizeData();

const SearchFilter = (props) => (
  <form onSubmit={props.searchFilter} onReset={props.ClearSearchData}>
    <div className="row mb-0">
      <CustomInput hasLabel={true} label="Company Code" divClassName="s12 m4" type='text'
        name='companyCode' autocomplete="off" colSize='s12 m4' inputClass="customInputs" onValueChange={props.handlerChange}
      />
      <CustomInput hasLabel={true} label={"Name"} divClassName="s12 m4"
        name='companyName' colSize='s12 m4 pl-0' inputClass="customInputs" autocomplete="off" onValueChange={props.handlerChange}
      />
      <CustomInput hasLabel={true} id="companyOperatingCountry" name='operatingCountry' divClassName='col' label={'Operating Country'} type='select'
        colSize='s12 m4 pl-0' className="browser-default" optionsList={props.countryMasterData} optionName='name' optionValue="id"
        onSelectChange={props.handlerChange}
      />
    </div>
    <div className="row mb-0">
      <CustomInput
        name="searchDocumentType"
        id="searchDocumentType"
        hasLabel={true}
        divClassName='col'
        label={'Search Documents'}
        type='select'
        colSize='s4'
        className="browser-default"
        optionName="name"
        optionValue="value"
        defaultValue=""
        optionsList={props.documentMasterData}
        onSelectChange={props.handlerChange}
      />
       <CustomInput name="documentSearchText" id="documentSearchText" hasLabel={true} label={"Search Document(s) For Words"} divClassName="col"
        colSize='s4 pl-0' autocomplete="off" inputClass="customInputs" onValueChange={props.handlerChange}
      />
      <div className="col s4 align-center mt-4x align-center pl-0">
        <button type="submit" id="companySearchClicked" className="modal-close waves-effect waves-green btn">Search</button>
        <button type="reset" className="modal-close waves-effect waves-green btn ml-2">Reset</button>
      </div>
    </div>
  
  </form>
);

class CompanyList extends Component {
  constructor(props) {
    super(props);
    this.updatedData = {};    
  }
  componentWillUnmount() {
    this.props.actions.ClearSearchData();
  }
  
  componentDidMount() {
    this.props.actions.ClearCompanyDetails();
    if(this.props.isSearch){
      this.props.actions.showHidePanel();
    }
   /**
    * commented FetchCountry and we are getting country list from masterdatareducer
    */
    // this.props.actions.FetchCountry();
    this.props.actions.FetchDocumentTypeMasterData();

    // document.getElementById("companyOperatingCountry").addEventListener("keydown", function(e) {
    //   if (e.key === "Enter" || (e.keyCode || e.which) === 13)
    //     // e.preventDefault();
    //     document.getElementById("companySearchClicked").click();
    // });
  }

  panelClick = (e) => {
    this.props.actions.showHidePanel();
  }

  handlerChange = (e) => {
    if(e.target.name === 'companyName')
    {
      this.updatedData[e.target.name] = e.target.value.replace("&", "%26");      
    }
    else if(e.target.name === 'operatingCountry'){
      this.updatedData[e.target.name]=e.target.value;
      this.updatedData['operatingCountryName']=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
    }
    else
    {
      this.updatedData[e.target.name] = e.target.value;
    }
  }

  searchFilter = (e) => {
    e.preventDefault();
    this.props.actions.FetchCompanyDataList(this.updatedData);

  }
  ClearSearchData=()=>{
    this.updatedData = {};
    this.props.actions.ClearSearchData(this.updatedData);
  }

  render() {
    const headData = HeaderData;
    const rowData = this.props.companyDataList;
    const countryMasterData = this.props.countryMasterData;   
    const { documentTypesData }=this.props;
    const masterDocData=documentTypesData&&documentTypesData.filter(x=>x.moduleName==="Company");
    return (
      <Fragment>
       
        <div className="companyPageContainer customCard">
          <Panel isArrow={true} colSize="s12" isopen={this.props.isopen} heading={`${ localConstant.companyDetails.SEARCH_COMPANY }`}  subtitle={` ${ localConstant.companyDetails.EDIT_VIEW_COMPANY }`} onpanelClick={this.panelClick}>
            <SearchFilter
             handlerChange={this.handlerChange} 
             ClearSearchData={this.ClearSearchData} 
             searchFilter={this.searchFilter}
             countryMasterData={countryMasterData}
             documentMasterData={masterDocData} >
             </SearchFilter>
          </Panel>
          <ReactGrid gridRowData={rowData} gridColData={headData} />
        </div>
      </Fragment>
    );
  }
}

export default CompanyList;
