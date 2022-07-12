import React, { Component, Fragment } from 'react';
import CardPanel from '../../../common/baseComponents/cardPanel';
import CustomInput from '../../../common/baseComponents/inputControlls';
import ReactGrid from '../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import LabelWithValue from '../../../common/baseComponents/customLabelwithValue';
import Modal from '../../../common/baseComponents/modal';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import ReactDiffViewer from 'react-diff-viewer';
import moment from 'moment';
import { getlocalizeData, uniq, formInputChangeHandler, isEmpty, bindAction } from '../../../utils/commonUtils';
const localConstant = getlocalizeData();
const SearchCriteria = (props) => {
    let displayName = [];
    const data = props.selectTypeData.filter(x => displayName = x.dispalyName);
    return (
        <Fragment>
            <CardPanel className="white lighten-4 black-text" title={"Search Criteria"} colSize="s12" >
            <form id="auditSearch"
                    onSubmit={props.onClickSearch}
                    autoComplete="off">
                <div className="row mb-0" >
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.auditSearch.MODULE}
                        type='select'
                        colSize='s4'
                        optionsList={props.moduleData && props.moduleData}
                        optionName="moduleName"
                        optionValue="module"
                        maxLength={200}
                        name="moduleName"
                        className="browser-default customInputs"
                        labelClass="mandate"
                        // defaultValue={props.editedRowData.auditModuleName}
                        onSelectChange={props.handlerChange}
                    />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0 '
                        label={"Search Type"}
                        type='select'
                        colSize='s4'
                        optionsList={displayName}
                        optionName="name"
                        optionValue="value"
                        maxLength={200}
                        name="selectType"
                        className="browser-default customInputs"
                        labelClass="mandate"
                       
                        defaultValue={props.updatedData.selectType}
                        onSelectChange={props.handlerChange}
                    />
                    <CustomInput
                        hasLabel={true}
                        label={localConstant.auditSearch.COLUMN_VALUE}
                        divClassName='col '
                        type='text'
                        dataValType="valueText"
                        colSize='s4'
                        inputClass="customInputs "
                        maxLength={200}
                        name="columnValue"
                        labelClass="mandate"
                        onValueChange={props.handlerChange}
                        value={props.colmnVal}
                        autocomplete="off"
                    />
                </div>
                <div className="row">
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0 '
                        label={localConstant.techSpec.professionalEducationalDetails.FROM}
                        type='date'
                        colSize='s3'
                        name="fromDate"
                        inputClass="customInputs"
                        labelClass="mandate"
                        placeholderText={localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT}
                        selectedDate={isEmpty(props.fromDate) ? isEmpty(props.toDate)?'':moment(props.toDate).subtract(1, 'months') : props.fromDate} // D - 794
                        onDateChange={props.fetchFromDate}
                        dateFormat={"DD-MMM-YYYY"}
                    />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.professionalEducationalDetails.TO}
                        type='date'
                        colSize='s3'
                        name="toDate"
                        labelClass="mandate"
                        inputClass="customInputs"
                        placeholderText={localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT}
                        selectedDate={props.toDate}
                        onDateChange={props.fetchToDate}
                        dateFormat={"DD-MMM-YYYY"}
                        // defaultValue={props.toDate}
                    />

                    <label className=" col mt-4 pt-3">
                        <input type="checkbox" className="filled-in" name="deletedItems" onClick={props.checkBoxHandler} defaultChecked={""} />
                        <span className="labelPrimaryColor">{localConstant.auditSearch.SHOW_DELETED_ITEMS}</span>
                    </label>
                    <div className="col pl-4 pt-1 ">
                    <button  type="submit" className=" btn-small modal-close waves-effect waves-green btn  mt-4x">Search</button>
                    </div>

                </div>
                </form>
            </CardPanel>
        </Fragment>

    );
};
const ParentModuleGrid = (props) => (

    <div className=" customCard">
        <ReactGrid gridColData={HeaderData.AuditSearchHeader} gridRowData={props.rowData}

            onSelectionChanged={props.onRowSelected}
            onRef={props.onRef}

        />
    </div >
);
const ParentSubModuleGrid = (props) => (
    <div className=" customCard">
        <Fragment>
            <ReactGrid gridColData={HeaderData.AuditSearchSubHeader} gridRowData={props.rowData} />
        </Fragment>
    </div>
);

class AuditSearch extends Component {
    constructor(props) {
        super(props);
        this.state = {
            editMode: false,
            selectedValue: '',
            colmnVal: '',
            fromDate: moment().subtract(1, 'months'), // D - 794
            toDate: moment(),
            todayDate: moment(),
            deletedItems:false,
        };
        this.updatedData = {};
        this.editedRowData = {};
        this.selectedRowData = [];
        this.selectTypeData = [];

        this.cancelButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelButton,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },

        ];
        bindAction(HeaderData.AuditSearchSubHeader, "EditColumn", this.editRowHandler);
    }
    handlerChange = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        if (inputvalue.name === 'columnValue') {
            this.setState({ colmnVal: (inputvalue.value) });
        }

        if (inputvalue.name === 'moduleName') {
            const tempModule = this.props.moduleData.find(module => module.module === inputvalue.value);
            if (tempModule){
                this.updatedData['moduleId'] = tempModule.moduleId;
            }
            this.props.actions.ClearParentData();
            this.setState({
                colmnVal: '',
                selectedValue: ''
            });
            this.updatedData.selectType = null;
            this.selectTypeData = this.props.moduleData.filter(x => x.module === this.updatedData.moduleName);

        }

        if (inputvalue.name === 'moduleName' || inputvalue.name === 'selectType') {
            this.updatedData.columnValue = null;
            this.setState({ colmnVal: '',selectedValue:'' });
            this.props.actions.ClearParentData();
        }
    }
    checkBoxHandler=(e)=>{
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    }
    fetchFromDate = (date) => {
        this.setState({ fromDate: date });
        this.updatedData.fromDate = this.state.fromDate !== null ? this.state.fromDate : '';
    }
    fetchToDate = (date) => {
        this.setState({ toDate: date });
        this.updatedData.toDate = this.state.toDate !== null ? this.state.toDate : '';
    }
    componentDidMount() {
        this.props.actions.FetchModuleName();
        this.props.actions.ClearParentData();
    }
    editRowHandler = (data) => {
        this.setState({ editMode: true });
        this.editedRowData = data;
    }
    cancelButton = (e) => {
        this.setState({ editMode: false });
        this.editedRowData = {};
    }
    onClickSearch = (e) => {
        e.preventDefault();
        this.props.actions.ClearParentData();
        if (isEmpty(this.updatedData.moduleName)) {
            IntertekToaster(localConstant.auditSearch.MODULE_VALIDATION, 'warningToast ');
            return false;
        }
        if (isEmpty(this.updatedData.selectType)) {
            IntertekToaster(localConstant.auditSearch.SELECT_TYPE_VALIDATION, 'warningToast ');
            return false;
        }
        if(isEmpty(this.state.fromDate) ){
            IntertekToaster(localConstant.auditSearch.FROM_DATE_VALIDATION, 'warningToast ');
            return false;
        }
        if (isEmpty(this.state.toDate)) {
            IntertekToaster(localConstant.auditSearch.TO_DATE_VALIDATION, 'warningToast ');
            return false;
        }
        if (isEmpty(this.updatedData.columnValue && this.updatedData.columnValue.trim()) && !isEmpty(this.updatedData.selectType)) { // D - 794
            if(this.updatedData.selectType==="ProjectAssignment" ){
                IntertekToaster(localConstant.auditSearch.PROJECT_ASSIGNMENT_VALIDATION, 'warningToast ');
                return false;
            }else{
            IntertekToaster(localConstant.auditSearch.COLUMN_VALUE_VALIDATION, 'warningToast ');
            return false;
            }
        }
        this.setState({ deletedItems:this.updatedData.deletedItems });
        if (this.updatedData.selectType === "ProjectAssignment" && !isEmpty(this.updatedData.columnValue)) {
            if (this.updatedData.columnValue.includes('-')) {

                if(isEmpty(this.updatedData.columnValue.split("-")[0])){
                    IntertekToaster(localConstant.auditSearch.PROJECT_NUMBER, 'warningToast ');
                    return false;
                }
                if(isEmpty(this.updatedData.columnValue.split("-")[1])){
                    IntertekToaster(localConstant.auditSearch.ASSIGNMENT_NUMBER, 'warningToast ');
                    return false;
                }
                
            } else {
                IntertekToaster(localConstant.auditSearch.PROJECT_ASSIGNMENT_VALIDATION, 'warningToast ');
                return false;
            }
        }
        this.setState({ selectedValue: [] });
        if (!this.dateRangeValidator(this.state.fromDate, this.state.toDate,this.state.todayDate) && !this.state.inValidDateFormat) {
           
                    const data = {
                        unquieId: this.updatedData.columnValue,
                        auditModuleName: this.updatedData.moduleName,
                        auditModuleId:this.updatedData.moduleId,
                        selectType:this.updatedData.selectType,
                        fromDate: isEmpty(this.state.fromDate) ? moment(this.state.toDate).subtract(1, 'months').format( 'YYYY-MM-DD') : this.state.fromDate.format( 'YYYY-MM-DD'), // D - 794
                        toDate: this.state.toDate.format( 'YYYY-MM-DD')
                    };
                    // this.props.actions.FetchAuditSearchData(data);
                    this.props.actions.FetchAuditEventData(data);

        }
    }
    dateRangeValidator = (from,to,todayDate) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from >to) {
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.INVALID_DATE_RANGE, 'warningToast');
            }
            else if(to>todayDate){
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.INVALID_CURRENTDATE, 'warningToast');
            }
        }
        return isInValidDate;
    }
    onGridRowSelected = async (e) => {
        const selectedRows = this.secondchild.getSelectedRows();

        const res = await this.props.actions.FetchAuditLogData(selectedRows[0].auditEventId);
        if (res) {
            res.forEach(x => {
                if (!isEmpty(x.oldValue) && !isEmpty(x.newValue) && isEmpty(x.diffValue)) {
                    this.selectedRowData.push([]);
                } else {
                    this.selectedRowData.push(x);
                }
            });

        }
        this.setState({
            selectedValue: this.selectedRowData
        });

        this.selectedRowData = [];
    }

    decodeHTML = (html) =>{
        const txt = document.createElement('textarea');
        txt.innerHTML = html;
        return txt.value;
    };

    jsonEscape=(str) => {
        return str.replace(/\r/g, "\\\\r").replace(/\t/g, '\\t');
    }
 
    render() {
        const parentGridData = this.props.auditSearchData;

        let ModuleData = this.props.moduleData;
        if (Array.isArray(ModuleData) && (this.props.moduleData).length > 0) {
            ModuleData = (this.props.moduleData).sort((a, b) => {
                return (a.moduleName < b.moduleName) ? -1 : (a.moduleName > b.moduleName) ? 1 : 0;
            });
        }
       
      return (
            <Fragment>
                <Modal
                    modalId="approveDocumentPopup" formId="sapproveDocumentForm"
                    modalClass="aduitModalWidth"
                    modalContentClass="maxModalHeight"
                    buttons={this.cancelButtons}
                    isShowModal={this.state.editMode}
                    title={localConstant.auditSearch.AUDIT_SEARCH}
                >
                   
                 {!isEmpty(this.props.auditSearchData) && !isEmpty(this.editedRowData.diffValue)? 
                                    <Fragment>         
                                    <table className="auditTable">
                                            <tr>
                                                <th>{localConstant.auditSearch.FIELD}</th>
                                                <th>{localConstant.auditSearch.OLD_VALUE}</th>
                                                <th>{localConstant.auditSearch.NEW_VALUE}</th>
                                            </tr>
                                                   { Object.entries(JSON.parse(this.jsonEscape(this.editedRowData.diffValue))).sort().map((keys,values)=>{
                                                      
                                            return (
                                                    <tr>
                                                        <td>{keys[0]}</td>
                                                        <td>{keys[1][0]===false||keys[1][0]===true?keys[1][0].toString():keys[1][0]}</td>
                                                        <td>{keys[1][1]===false||keys[1][1]===true?keys[1][1].toString():keys[1][1]}</td>
                                                    </tr>                        
                                            );
                                            })
                                    }
                                    </table>    
                                        </Fragment> :
                                        !isEmpty(this.editedRowData)&& !isEmpty(this.props.auditSearchData)   ?         
                                    isEmpty(this.editedRowData.oldValue)?
                                    <Fragment>    
                                    <table className="auditTable">
                                            <tr>
                                            <th>{localConstant.auditSearch.FIELD}</th>
                                                <th>{localConstant.auditSearch.OLD_VALUE}</th>
                                                <th>{localConstant.auditSearch.NEW_VALUE}</th>
                                            </tr>
            
                                                   { Object.entries(JSON.parse(this.jsonEscape(this.editedRowData.newValue))).sort().map((keys,values)=>{
                                          if(!isEmpty(keys[1])){
                                              
                                            return (
                                                <tr>
                                                   
                                                    <td>{keys[0]}</td>
                                                    <td></td>
                                                    <td>{keys[1]===false ||keys[1]===true ?this.decodeHTML(keys[1].toString()):this.decodeHTML(keys[1])}</td>
                                                </tr>
                                        );
                                          }
                                            })
                                    }
                                    </table>
                                        </Fragment>:<Fragment> 
                                        <table className="auditTable">
                                                <tr>
                                                <th>{localConstant.auditSearch.FIELD}</th>
                                                <th>{localConstant.auditSearch.OLD_VALUE}</th>
                                                <th>{localConstant.auditSearch.NEW_VALUE}</th>
                                                </tr>
                                                       { Object.entries(JSON.parse(this.jsonEscape(this.editedRowData.oldValue))).sort().map((keys,values)=>{
                                            if(!isEmpty(keys[1])){
                                                return (
                                                    <tr>
                                                        <td>{keys[0]}</td>
                                                        <td>{keys[1]===false||keys[1]===true ?keys[1].toString():keys[1]}</td>
                                                        <td></td>
                                                    </tr>
                                            );
                                            }
                                                })
                                        }
                                        </table> 
                                            </Fragment>   :''
                                }
                </Modal>
                <div className=" customCard">
                    <SearchCriteria handlerChange={this.handlerChange}
                        moduleData={ModuleData}
                        submoduleData={this.props.submoduleData}
                        onClickSearch={ (e) => this.onClickSearch (e)}
                        colmnVal={this.state.colmnVal}
                        updatedData={this.updatedData}
                        selectTypeData={this.selectTypeData}
                        fetchFromDate={this.fetchFromDate}
                        fetchToDate={this.fetchToDate}
                        fromDate={this.state.fromDate}
                        toDate={this.state.toDate}
                        checkBoxHandler ={this.checkBoxHandler}
                    />
                    <ParentModuleGrid
                        rowData={this.state.deletedItems?parentGridData.filter(x=>x.actionType==='D'):parentGridData}
                        onRowSelected={this.onGridRowSelected}
                        onRef={ref => { this.secondchild = ref; }}
                        paginationPrefixId={localConstant.paginationPrefixIds.auditParentGridId}
                    />
                    <ParentSubModuleGrid
                        rowData={this.state.deletedItems && !isEmpty( this.state.selectedValue) ?
                            this.state.selectedValue.filter(x=> x.actionType==='D' && isEmpty(x.newValue))  
                            :!isEmpty(this.state.selectedValue) && !isEmpty(this.props.auditSearchData) ? this.state.selectedValue.filter(x=>x.length!==0) : []}       
                            paginationPrefixId={localConstant.paginationPrefixIds.auditChildGridId}
                    />
                </div>

            </Fragment>
        );
    }
}
export default AuditSearch;