import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../common/baseComponents/reactAgGrid';
import CustomInput from '../../../common/baseComponents/inputControlls';
import Modal from '../../../common/baseComponents/modal';
import { getlocalizeData, isEmpty } from '../../../utils/commonUtils';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import PropTypes from 'prop-types';
const localConstant = getlocalizeData();
const SearchModal = (props) => (
    <div className="row mb-0">
        <CustomInput hasLabel={true}
            name={props.name ? props.name :'customerName'}
            divClassName='col' 
            label={localConstant.companyDetails.generalDetails.NAME}
            type='text'
            colSize='s6'
            inputClass="customInputs"
            dataValType='valueText'
            onValueChange={props.OnModalInputChange}
            onValueBlur={props.onModalInputBlur}
            onValueKeyDown={props.handlerKeyDown}
            value={props.serachInput}  //Def 752
            autocomplete="off" />
        <CustomInput 
            hasLabel={true}
            name='operatingCountry'
            divClassName='col'
            label={localConstant.modalConstant.COUNTRY}
            type='select'
            colSize='s6'
            inputClass="customInputs"
            optionsList={props.countryMasterData}
            optionName='name'
            optionValue="name"
            className="browser-default"
            onSelectChange={props.selectedCountrySearch}
            defaultValue={props.defaultCountryName} />
    </div>

);
class InputWithPopUpSearch extends Component {
    constructor(props) {
        super(props);
        this.state = {  
            CustomerSearchModalOpen:false,
            serachInput: null, //Def 752
            selectedCountry:''
        };
        this.updatedInputData = {};
        this.selectedcustomerName = [];
        if(props.callBackFuncs){
            props.callBackFuncs.onReset =()=>{
                this.clearSupplierData();
            };
            props.callBackFuncs.onCancel =()=>{
                this.onCancel();
            };
        }

        if(props.callBackFunc){
            props.callBackFunc.onReset =()=>{
                this.clearSupplierData();
            };
        }
    } 
// To show the supplierPOP up by toggling the state property at the time of Did Mount. 
    componentDidMount()
    {
        //based on this property will show supplierSearchModal state.
        if(this.props.isShowSupplierModal)
        {
        this.setState ( { CustomerSearchModalOpen : true } );
        }
        //to clear the search grid datas on SupplierPO Module
        if(this.props.isShowSupplierSearchButton)
        {
        this.props.ClearSearchResult();
        }
    }

    onCancel=()=>{
        this.setState({ serachInput: this.props.defaultInputValue });
    }
    handleSearchBoxChange = (e) => {    
        const inpVal = this.state.serachInput;   
        if(inpVal && (e.keyCode === 9 || e.keyCode === 13))
        {
            if(inpVal[inpVal.length - 1] != '*'){
                this.setState({
                    serachInput:inpVal +'*',
                    CustomerSearchModalOpen:true
                },()=>this.props.onInputBlur(this.state));
         }
         else{
            this.setState({
                serachInput:inpVal,
                CustomerSearchModalOpen:true
            },()=>this.props.onInputBlur(this.state));
         }
         if(e.keyCode===13)
         e.preventDefault();
            } 

    }
    handlerChange = (e) => { 
        e.preventDefault(); 
        if(!e.target.value)
        {
            this.setState({
                selectedCountry:''
            });
            this.props.handleInputChange && this.props.handleInputChange(e);
        }
        
        this.setState({
            serachInput:e.target.value 
        }); 

    }
    //Clear Search Data
    clearSearchData = () => {
        this.updatedInputData = {};
        // this.props.actions.ClearSearchData();
        //TO Clear the Search Grid Datas on supplierPO module
        if(this.props.isShowSupplierSearchButton)
        {
        this.props.ClearSearchResult();
        }
        this.setState({
            CustomerSearchModalOpen:false,
            serachInput:'', //D744 related issue fixing  //UnCommented for D-932
            selectedCountry:'' 
        });
    }
    clearSupplierData = () =>
    {
        this.setState({
            serachInput:'' 
        });
    }
  
    //selected customer Country Change
    selectedCustomerCountrySearch = (e) => {
        e.preventDefault();
        this.setState({
            selectedCountry:e.target.value
        },()=>this.props.onModalSelectChange(this.state));
    }

    //selected search on Text Box Onchange 
    OnModalInputChange = (e) => {
        e.preventDefault();
        this.setState({
            serachInput:e.target.value
        });
      
    }

    onModalInputBlur =(e) => {
        e.preventDefault();
         if(this.state.serachInput &&  this.state.serachInput.lastIndexOf("*") <= 0){
        this.setState({
            serachInput:e.target.value +"*"
        });
    }
    else {
                this.setState({
                    serachInput:e.target.value
                });
            }
    }

    //onButton Click Open Modalpoup
    selectCustomerSearch = (e) => {
        e.preventDefault();
        const inpVal = this.state.serachInput; 
        if(inpVal &&  inpVal[inpVal.length - 1] !='*' ){
            this.setState({
                serachInput:inpVal +'*'
            },()=>this.props.onInputBlur(this.state));
            }
            else if(inpVal && inpVal[inpVal.length - 1] =='*')
            {
                this.setState({
                    serachInput:inpVal
                },()=>this.props.onInputBlur(this.state));
            }  
            // else if(this.props.defaultInputValue){  //Added for D-479 issue2 /**commented for D744 related issue */
            //     this.setState({
            //         serachInput:this.props.defaultInputValue 
            //     },()=>this.props.onInputBlur(this.state));
            // }

        this.props.onAddonBtnClick(this.state);

        this.setState({
            CustomerSearchModalOpen:true
        });
    }   

    //get customer Name From Popup AG Grid 
    getCustomerName = (e) => {
        e.preventDefault();
        const selectedData = this.child.getSelectedRows();
        if (this.props.gridRowData && this.props.gridRowData.length > 0) {
            if (isEmpty(selectedData)) {
                IntertekToaster(localConstant.commonConstants.PLEASE_SELECT_ATLEAST_ONE_RECORD, 'warningToast customerSearchCustomerNameReq');
            }
            else {
                this.props.onSubmitModalSearch(selectedData);
                //To hide the Supplier Modal on submit
                if(this.props.isShowSupplierSearchButton)
                {
                    this.props.cancelSubSupplierPopUp();
                }
                    this.setState({
                    CustomerSearchModalOpen:false,
                    serachInput:selectedData[0] && selectedData[0][this.props.objectKeySelector],
                    selectedCountry: selectedData[0] && selectedData[0].country  
                });
            }
            
        }
    }
   
    hideModal = (e) => {
        e.preventDefault();      
        // this.props.actions.HideModalPopup();
        this.updatedInputData = {};
        //TO hide the supplier Modal 
        if(this.props.isShowSupplierSearchButton)
        {
        this.props.cancelSubSupplierPopUp();
        }
        if(this.props.cancelSupplierVisit){
            this.props.cancelSupplierVisit();
        }
        this.clearSearchData();
        this.setState({
            CustomerSearchModalOpen:false
        });
    }

    handleModalSearchBox = (e) => {      
        if(e.keyCode === 9 || e.keyCode === 13){
            const inpVal = e.target.value;
            if(inpVal[inpVal.length - 1] !== '*'){
                this.setState({
                    serachInput:inpVal +'*'
                },()=>this.props.onModalSelectChange(this.state));
                }
                else{
                    this.setState({
                        serachInput:inpVal
                    },()=>this.props.onModalSelectChange(this.state));
                }
            } 
            if(e.keyCode === 13)
            e.preventDefault();
    }
    //Def 752
    static getDerivedStateFromProps(props, state) {
        if ((!state.serachInput)  && props.defaultInputValue) {
            return {
                serachInput: props.defaultInputValue
            };
           }
        // when null is returned no update is made to the state
        return null;
      }
    render() {
        const {  interactionMode,isShowSupplierSearchButton } =this.props; 
        return (
            <Fragment>
                {this.state.CustomerSearchModalOpen?
                <Modal id="contractModalPopup"
                    title={this.props.searchModalTitle}
                    modalClass="contractModal"
                    buttons={[
                        {
                            name: 'Cancel', action: this.hideModal,
                            type: "reset",
                            btnClass: 'btn-small mr-2',
                            showbtn: true
                        },
                        {
                            name: 'Submit',
                            type: "submit",
                            action: this.getCustomerName,
                            btnClass: 'btn-small',
                            showbtn: true
                        }
                    ]}
                    isShowModal={this.state.CustomerSearchModalOpen}>
                    <SearchModal 
                        countryMasterData={this.props.countryMasterData}
                        OnModalInputChange={this.OnModalInputChange}
                        onModalInputBlur={this.onModalInputBlur}
                        defaultInputValue={this.props.defaultInputValue }
                        handlerChange={this.handlerChange}
                        handlerKeyDown={this.handleModalSearchBox}
                        selectedCountrySearch={this.selectedCustomerCountrySearch}
                        defaultCountryName={this.state.selectedCountry}
                        name={this.props.name}
                        serachInput = { this.state.serachInput } />
                    <ReactGrid 
                        gridRowData={this.props.gridRowData}
                        gridColData={this.props.headerData} onRef={ref => { this.child = ref; }}
                        columnPrioritySort={this.props.columnPrioritySort} /> 
                </Modal>:null}
                {/* To show the Supplier search button based on the props */}
                {  ! isShowSupplierSearchButton  ?
                    <div className={this.props.colSize?this.props.colSize :"col s4 pl-0"} >
                        <CustomInput
                            dataValType='valueText'
                            hasLabel={true}
                            divClassName='col customerSearchBox'
                            label={this.props.label}  
                            labelClass={this.props.className}                          
                            type='text'
                            colSize={this.props.searchcolSize?this.props.searchcolSize:'s11 pr-0'}
                            name={this.props.name ? this.props.name :'customerName'}
                            inputClass="customInputs"
                            maxLength={200}
                        value={ this.state.serachInput }  //Def 752
                            onValueFocus={this.handlerFocus}
                            onValueKeyDown={this.handleSearchBoxChange}
                            onValueChange={this.handlerChange} 
                            readOnly={interactionMode}
                            autocomplete="off"
                            />
                        <div className="customerSearchButton">
                        <button type="button" className="waves-effect waves-green btn p-2 btn-lineHeight mt-4x" onClick={this.selectCustomerSearch}  disabled={interactionMode} >...</button>
                        </div>
                </div>
              :null }
            </Fragment> 
        );
    }
}

export default InputWithPopUpSearch;

InputWithPopUpSearch.propTypes = {
    isShowSupplierSearchButton: PropTypes.bool,

};

InputWithPopUpSearch.defaultProps = {
    isShowSupplierSearchButton: false
};
