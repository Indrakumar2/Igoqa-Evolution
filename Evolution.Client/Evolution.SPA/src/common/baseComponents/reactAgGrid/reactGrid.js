import React, { Component, Fragment } from 'react';
import { AgGridReact } from 'ag-grid-react';
import MaterializeComponent from 'materialize-css';
import { connect } from "react-redux";
import CustomerAnchorRenderer from '../../../components/viewComponents/customer/customerAnchor';
import ContractAnchorRenderer from '../../../components/viewComponents/contracts/contractAnchor';
import MySearchRenderer from '../../../grm/components/techSpec/mySearch/mySearchAnchor';
import ProjectAnchorRenderer from '../../../components/viewComponents/projects/projectAnchor';
import CompanyAnchor from '../../../components/viewComponents/company/comapnyAnchor';
import HyperLink from '../hyperLink';
import FileToBeOpen from '../../commonDocumentDownload';
import ReportToDownload from '../../commonReportDownload';
import FileToBeDownload from '../../../grm/downloadDocument/downloadDocument';
import DateComponent from '../date';
import EditRenderer from '../editRenderer';
import EditLink from '../editLink';
import ExportLink from '../exportLink';
import SelectChargeValue from '../../../components/applicationComponents/contracts/rateSchedule/selectChargeValue';
import { AddNewInvoiceDefault } from '../../../actions/contracts/invoicingDefaultsAction';
import ProjectAnchor from '../../../components/viewComponents/projects/projectAnchor';
import supplierAnchorRenderer from '../../../components/viewComponents/supplier/supplierAnchor';
import SupplierpoAnchor from '../../../components/viewComponents/supplierpo/supplierpoAnchor';
import AssignmentAnchor from '../../../components/viewComponents/assignment/assignmentAnchor';
import VisitAnchor from '../../../components/viewComponents/visit/visitAnchor';
import TimesheetAnchor from '../../../components/viewComponents/timeSheet/timesheetAnchor';
import AgGridDateFilter from '../agGridDateFilter';
import SelectDocumentType from '../../selectDocumentType';
import SelectType from '../../inlineEditVisitTimesheet/selectType';
import SelectDataType from '../../resourcePayRate/selectDataType';
import SelectContractRate from '../../contractRate/selectContractRate';
import { customComparator } from '../../../utils/arrayUtil';
import moment from 'moment';

/** GRM Imports */
import GridEditRender from '../../../grm/components/applicationComponent/gridEditRender';
import HyperLinkRenderer from '../../../grm/components/applicationComponent/hyperLinkAnger';
import CustomStarRating from '../../../grm/components/techSpec/resourceCapability/ratingstar';
import OverrideApprovalLink from '../../../components/applicationComponents/assignment/arsSearch/overrideApprovalLink';
import DocumentationLink from '../../../components/applicationComponents/dashboard/documentation/documentationLink';
import PrintReport  from '../../../common/commonPrintReport';
import { func } from 'prop-types';

/** Inline Editing components */
import InlineSwitch from '../../baseComponents/inlineComponents/switch';
import InlineSelect from '../../baseComponents/inlineComponents/select';
import InlineTextbox from '../../baseComponents/inlineComponents/textbox';
import InlineCheckbox from '../../baseComponents/inlineComponents/checkbox';
import { getlocalizeData,isEmpty } from '../../../utils/commonUtils';
import { required } from '../../../utils/validator';

const localconstant = getlocalizeData();

/** Admin Grid HyperLink */
//import adminHyperLinkRenderer from '../../../grm/components/applicationComponent/hyperLinkAnger';

class ReactGrid extends Component {

    constructor(props) {
        super(props);
        this.paginationPrefixId = required(props.paginationPrefixId) ? "" : props.paginationPrefixId;
        this.getNodeChildDetails = this.getNodeChildDetails.bind(this);      
        this.state = {
            paginationPageSize: 10,
            txtCurrentPage:0,
            lbTotalPages:0,
            lbTotalPagesTwo:0,
            currentPage:true,
            txtCurrentPageTwo:0,
            searchtxtbox:"",
            rowClassRules: {
                "dangerTag": (this.props.rowName === 'BudgetMonetary' ? (this.props.rowClassRules && this.props.rowClassRules.allowDangerTag ?
                    this.props.rowClassRules && this.props.rowClassRules.allowDangerTag : false) && function (params) {

                        if (params.data.isOverBudgetValue === true) {
                            return true;
                        }

                    }
                    // :this.props.rowName === 'subSupplierTS' ? (this.props.rowClassRules && this.props.rowClassRules.allowDangerTag ?
                    //     this.props.rowClassRules && this.props.rowClassRules.allowDangerTag : false) && function (params) {
                    //         const inActiveList = localconstant.commonConstants.IN_ACTIVE_TS;
                    //         if (inActiveList && inActiveList.includes(params.data.profileStatus)) {
                    //             return true;
                    //         }
                    //     }
                    : this.props.rowName === 'BudgetHours' ? (this.props.rowClassRules && this.props.rowClassRules.allowDangerTag ?
                        this.props.rowClassRules && this.props.rowClassRules.allowDangerTag : false) && function (params) {

                            if (params.data.isOverBudgetHour === true) {
                                return true;
                            }

                        } : (this.props.rowClassRules && this.props.rowClassRules.allowDangerTag) && function (params) {

                                //AssignedSpecialist Grid For Assignement(D578)
                                if(params.data.InActiveDangerTag !== undefined && params.data.InActiveDangerTag === true){
                                    return true;
                                }

                                if (params.data.techSpecialists !== undefined || params.data.assignmentOperatingCompanyCoordinator !== undefined) {
                                    if (params.data.techSpecialists !== undefined) {
                                        if ((params.data.techSpecialists === null) || params.data.techSpecialists.length === 0) {
                                            return true;
                                        } else if ((params.data.techSpecialists).trim() === '') {
                                            return true;
                                        }
                                    }
                                    if (params.data.assignmentOperatingCompanyCoordinator !== undefined) {
                                        if ((params.data.assignmentOperatingCompanyCoordinator === null) || (params.data.assignmentOperatingCompanyCoordinator.length === 0)) {
                                            return true;
                                        } else if ((params.data.assignmentOperatingCompanyCoordinator).trim() === '') {
                                            return true;
                                        }
                                    }

                                }
                                else if (params.data.techSpecialists === undefined || params.data.assignmentOperatingCompanyCoordinator === undefined) {
                                    return false;
                                }

                            }),

                "defaultTag": function (params) {
                    return params.data.techSpecialists === undefined;
                },
                "oddRowColor": function (params) {
                    if (params.node.rowIndex % 2 === 0) {
                        return true;
                    }
                },
                "rowHighlight":
                    (this.props.rowClassRules && props.draftData && this.props.rowClassRules.rowHighlight ?
                        this.props.rowClassRules && this.props.rowClassRules.rowHighlight : false) && function (params) {
                            if (params) {
                                return props.draftData(params);
                            } else {
                                return false;
                            }

                        },
            },
            frameworkComponents: {
                customerAnchorRenderer: CustomerAnchorRenderer,
                contractAnchorRenderer: ContractAnchorRenderer,
                projectAnchorRenderer: ProjectAnchorRenderer,
                companyAnchorRenderer: CompanyAnchor,
                HyperLink: HyperLink,
                DateComponent: DateComponent,
                EditRenderer: EditRenderer,
                EditLink: EditLink,
                ExportLink:ExportLink,
                GridEditRender: GridEditRender,
                HyperLinkRenderer: HyperLinkRenderer,
                FileToBeOpen: FileToBeOpen,
                ReportToDownload: ReportToDownload,
                PrintReport:PrintReport,
                FileToBeDownload: FileToBeDownload,
                SelectChargeValue: SelectChargeValue,
                ProjectAnchor: ProjectAnchor,
                supplierAnchorRenderer: supplierAnchorRenderer,
                SupplierpoAnchor: SupplierpoAnchor,
                AssignmentAnchor: AssignmentAnchor,
                CustomStarRating: CustomStarRating,
                mySearchRenderer: MySearchRenderer,
                VisitAnchor: VisitAnchor,
                TimesheetAnchor: TimesheetAnchor,
                OverrideApprovalLink: OverrideApprovalLink,
                DocumentationLink: DocumentationLink,
                agDateInput: AgGridDateFilter,
                SelectDocumentType: SelectDocumentType,
                SelectType: SelectType,
                SelectDataType: SelectDataType,
                SelectContractRate: SelectContractRate,
                /** Inline Components */
                InlineSwitch:InlineSwitch,
                InlineSelect:InlineSelect,
                InlineTextbox:InlineTextbox,
                InlineCheckbox:InlineCheckbox,
               
            },
            overlayLoadingTemplate: "<div class='loaders'><div></div><div></div><div></div></div>",
            overlayNoRowsTemplate: "<span class='noRecords'>No Records Found</span>",

            defaultColDef: {
                sortable: true,
                resizable: true,
                headerCheckboxSelection: (this.props.gridColData.enableSelectAll === undefined ? false : this.props.gridColData.enableSelectAll) ? this.isFirstColumn : false,
                checkboxSelection: (this.props.gridColData.enableSelectAll === undefined ? false : this.props.gridColData.enableSelectAll) ? this.isFirstColumn : false,
            },
            rowData: null
        };
        this.props.gridColData.columnDefs.forEach(column => {
            if (column.checkboxSelection === undefined && column.buttonAction === undefined && column.cellRenderer === undefined)
                column["suppressNavigable"] = true; 
                column["comparator"]=customComparator;
        });
        this.Yscroll=0;
    }
    
    isFirstColumn = (params) => {
        const displayedColumns = params.columnApi.getAllDisplayedColumns();
        const thisIsFirstColumn = displayedColumns[0] === params.column;
        return thisIsFirstColumn;
    }

    onGridReady = (params) => {  
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        if(this.gridApi){
            if ((this.props && this.props.gridTwoPagination) && this.gridApi.paginationGetTotalPages() === 0) {
                setCurrentPageTwo(this.gridApi.paginationGetCurrentPage(),this.paginationPrefixId);
                setTotalPageTwo(this.gridApi.paginationGetTotalPages(),this.paginationPrefixId);
            } else if ((this.props && this.props.gridTwoPagination) && this.gridApi.paginationGetTotalPages() !== 0) {
                setCurrentPageTwo(this.gridApi.paginationGetCurrentPage() + 1,this.paginationPrefixId);
                setTotalPageTwo(this.gridApi.paginationGetTotalPages(),this.paginationPrefixId);
            }
            else {
                if (this.gridApi.paginationGetTotalPages() === 0) {
                        setCurrentPage(this.gridApi.paginationGetCurrentPage(),this.paginationPrefixId);
                } else {
                        setCurrentPage(this.gridApi.paginationGetCurrentPage() + 1, this.paginationPrefixId);
                }
                    setTotalPage(this.gridApi.paginationGetTotalPages(), this.paginationPrefixId);
            }
            if (this.props && this.props.columnPrioritySort) {
                this.gridApi.setSortModel(this.props.columnPrioritySort);
            }
        }
    }
    onRowDataChanged = () => {      
        if(document.querySelector(`#${ this.paginationPrefixId }page-size`)!==null){
            const value = parseInt(document.querySelector(`#${ this.paginationPrefixId }page-size`).value);
            if(this.gridApi){
                if(value >= this.props.gridRowData.length){
                    this.gridApi.paginationSetPageSize(Number(value));
                } else{
                    this.gridApi.paginationSetPageSize(Number(value));
                }   
            }        
        }
        //this.gridApi && this.gridApi.paginationSetPageSize(Number(10));
        this.gridApi && this.gridApi.setQuickFilter(null);   
        this.setState({ searchtxtbox:"" });    
    }
    onPaginationChanged = () => {               
        if ((this.props && this.props.gridTwoPagination) && this.gridApi) {
            if(document.querySelector(`#${ this.paginationPrefixId }page-size`)!==null){
                const value = parseInt(document.querySelector(`#${ this.paginationPrefixId }page-size`).value);
                if(value >= this.props.gridRowData.length){
                    this.gridApi.paginationSetPageSize(Number(value));
                }               
            } 
                setTotalPageTwo(this.gridApi.paginationGetTotalPages(),this.paginationPrefixId);
            if (this.gridApi.paginationGetTotalPages() === 0) {
                setCurrentPageTwo(this.gridApi.paginationGetCurrentPage(),this.paginationPrefixId);
            } else {
                setCurrentPageTwo(this.gridApi.paginationGetCurrentPage() + 1,this.paginationPrefixId);
            }
        }
        else if (this.gridApi) {        
            if(document.querySelector(`#${ this.paginationPrefixId }page-size`)!==null){
                const value = parseInt(document.querySelector(`#${ this.paginationPrefixId }page-size`).value); 
                if(!isEmpty(this.props.gridRowData) && value >= this.props.gridRowData.length){
                    this.gridApi.paginationSetPageSize(Number(value));
                }               
            }           
                setTotalPage(this.gridApi.paginationGetTotalPages(), this.paginationPrefixId);
            if (this.gridApi.paginationGetTotalPages() == 0) {
                setCurrentPage(this.gridApi.paginationGetCurrentPage(), this.paginationPrefixId);
            } else {
                setCurrentPage(this.gridApi.paginationGetCurrentPage() + 1, this.paginationPrefixId);
            }
            if(this.props.onCurrentPageChanged>=0 && this.state.currentPage) { //Changes for D1039
                let value = 1;
                if(document.querySelector(`#${ this.paginationPrefixId }page-size`)){
                     value = parseInt(document.querySelector(`#${ this.paginationPrefixId }page-size`).value);
                } 
                const currentPage =Math.floor(this.props.onCurrentPageChanged/value)+1;
                if(currentPage>this.gridApi.paginationGetTotalPages()){
                    setCurrentPage(this.gridApi.paginationGetTotalPages(), this.paginationPrefixId);
                    this.gridApi.paginationGoToPage(Number(this.gridApi.paginationGetTotalPages()-1));
                }
                else{
                    setCurrentPage(currentPage, this.paginationPrefixId);
                    this.gridApi.paginationGoToPage(Number(currentPage-1));
                }
            }
        }
        this.setState({ currentPage:true });
    }

    moveToFirstPage = () => {
        this.gridApi.paginationGoToFirstPage();
    }

    onBtnPrevious = () => {
        this.setState({ currentPage:false });
        this.gridApi.paginationGoToPreviousPage();
    }

    onBtnNext = () => {
        this.setState({ currentPage:false });
        this.gridApi.paginationGoToNextPage();
    }

    onQuickFilterChanged = (e) => {
        this.setState({ searchtxtbox:e.target.value });
        this.gridApi.setQuickFilter(e.target.value);
    }

    currentPageChanged = (e) => {
        this.setState({ currentPage:false });
        if(e.target.value !==""){
            this.gridApi.paginationGoToPage(Number(e.target.value - 1));
        }else{
            this.setState({ txtCurrentPage:'' });
        }        
    }

    checkEnterKey = (e) => {
        const code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) { //Enter keycode
            this.gridApi.paginationGoToPage(Number(e.target.value - 1));
        }
    }

    onPageSizeChanged = (newPageSize) => { 
        const value = newPageSize.target.value;
        this.gridApi.paginationSetPageSize(Number(value));
    }

    componentDidMount() {
        const select = document.querySelectorAll('select');
        const selectInstances = MaterializeComponent.FormSelect.init(select);
        const tooltips = document.querySelectorAll('.tooltipped');
        const tooltipInstances = MaterializeComponent.Tooltip.init(tooltips);
        this.props.onRef && this.props.onRef(this);        
        //Defect ID 507 Scroll Event added
        const prefixElement = document.getElementById(this.props.paginationPrefixId) && document.getElementById(this.props.paginationPrefixId).querySelectorAll(".ag-body-viewport");
        const elmnt = this.props.paginationPrefixId ? prefixElement : document.getElementsByClassName("ag-body-viewport");      
        elmnt[0].addEventListener("scroll", ()=>this.handleScroll(elmnt));  
    };
    //Defect ID 507 added Scroll Handle function
    handleScroll=(elmnt)=>{
            const currentPosition = elmnt[0].scrollTop;
             if(elmnt[0].scrollTop < elmnt[0].scrollHeight - elmnt[0].clientHeight){       
                this.Yscroll = currentPosition;
             }else{
                elmnt[0].scrollTop = this.Yscroll;
             }
    }
    componentWillUnmount() {
        this.props.onRef && this.props.onRef(undefined);  
        const prefixElement = document.getElementById(this.props.paginationPrefixId) && document.getElementById(this.props.paginationPrefixId).querySelectorAll(".ag-body-viewport");
        const elmnt = this.props.paginationPrefixId ? prefixElement : document.getElementsByClassName("ag-body-viewport");    
        elmnt[0].removeEventListener("scroll", ()=>this.handleScroll(elmnt)); //Defect ID 507 Reset Scroll EventListener
        this.Yscroll = 0; //Defect ID 507 Reset Scroll Position 
    }
    getSnapshotBeforeUpdate(prevProps, prevState){
        //Defect ID 507 Scroll bar issues added Javascript Code
        // const elmnt = document.getElementsByClassName("ag-body-viewport");
        // if( elmnt[0].scrollTop < this.Yscroll){
        //     elmnt[0].scrollTop = this.Yscroll;
        // }
        return this.Yscroll;
    }

    componentDidUpdate(prevProps, prevState, snapshot){
        const prefixElement = document.getElementById(this.props.paginationPrefixId) && document.getElementById(this.props.paginationPrefixId).querySelectorAll(".ag-body-viewport");
        const elmnt = this.props.paginationPrefixId ? prefixElement : document.getElementsByClassName("ag-body-viewport");
        if(elmnt && elmnt.length > 0 && elmnt[0]){
            if(this.state.currentPage && (prevState.currentPage !== this.state.currentPage)){
                elmnt[0].scrollTop = 0;
                elmnt[0].scrollLeft = 0;
            } else{
                elmnt[0].scrollTop = snapshot;
            }
        }
    }

    exportToCSV = () => {
        const params = {
            fileName: this.props.gridColData.exportFileName ? this.props.gridColData.exportFileName : "Intertek Document",
            columnKeys: []
        };

        if(this.props.gridColData.customHeader){
            params.customHeader=this.props.gridColData.customHeader;
        }
        const listOfColumns = this.props.gridColData.columnDefs;
        if (listOfColumns) {
            for (let column = 0; column < listOfColumns.length; column++) {
                if (listOfColumns[column].field && listOfColumns[column].hide !== true) {
                    params.columnKeys.push(listOfColumns[column].field);
                }
            }
        }
        params.processCellCallback = function (params) {
            if (params.column.colDef.cellRenderer === "DateComponent" && params.value !== "" && params.value !== null) {
                return moment(params.value).format('DD/MM/YYYY');
            } else {
                return params.value;
            }
        };

        this.gridApi.exportDataAsCsv(params);
    }

    getSelectedRows = () => {
        const selectedData = this.gridApi && this.gridApi.getSelectedRows();
        return selectedData;
    }
    getAllRows() {
        const rowData = [];
        this.gridApi.forEachNode(node => rowData.push(node.data));
        return rowData;
    }

    removeSelectedRows = (data) => {
        this.gridApi.updateRowData({ remove: data });
    }

    renderGridSearchBox = () => {
        const gridLength = this.props.gridRowData ? this.props.gridRowData.length : 0;
        if (gridLength > 0) {
            return (<div className={this.props.gridColData.searchable ? "input-field col s12 m4" : "hide"}>
                <input type="text" onChange={this.onQuickFilterChanged} className="customInputs browser-default searchtxtbox"  value={this.state.searchtxtbox} id="quickFilter" placeholder="Free Text Search.." />
            </div>);
        }
        else {
            return ("");
        }
    }

    renderPagination = () => {          
        let gridLength = 0;
        if (!this.props.gridColData.Grouping)
            gridLength = this.props.gridRowData ? this.props.gridRowData.length : 0;
        else {

            let count = 0;

            this.props.gridRowData.map(gridRow => {
                if (this.props.gridColData.GroupingDataName && gridRow[this.props.gridColData.GroupingDataName])
                    count = count + gridRow[this.props.gridColData.GroupingDataName].length;
                if (gridRow.childArray) {
                    gridRow.childArray.map(childRow => {
                        if(childRow[this.props.gridColData.GroupingDataName])
                            count = count + childRow[this.props.gridColData.GroupingDataName].length;
                    });

                }
            });

            this.props.gridRowData.map(gridRow => {
                if (this.props.gridColData.GroupingDataName && gridRow[this.props.gridColData.GroupingDataName])
                    count = count + gridRow[this.props.gridColData.GroupingDataName].length;
                if (gridRow.childArray) {
                    gridRow.childArray.map(childRow => {
                        if(childRow[this.props.gridColData.GroupingDataName])
                            count = count + childRow[this.props.gridColData.GroupingDataName].length;
                    });

                }
            });

            gridLength = this.props.gridRowData ? count : 0;
        }

        if ((this.props && this.props.gridTwoPagination) && gridLength > this.state.paginationPageSize) {
            return (
                <div>
                    <div className={this.props.gridColData.pagination ? 'input-field col m2 pagedropdownCount ' : 'hide'}>
                        <select onChange={this.onPageSizeChanged} id={ `${ this.paginationPrefixId }page-size` } className='browser-default'>
                            <option value="10" selected defaultValue>10 Rows</option>
                            <option value="25">25 Rows</option>
                            <option value="50">50 Rows</option>
                            <option value="100">100 Rows</option>
                            <option value="500">500 Rows</option>
                            <option value={gridLength} >Show All</option>
                        </select>
                    </div>
                    <div className={this.props.gridColData.pagination ? "input-field col pr-0 mr-0 pagination" : "hide"}>
                        <a onClick={this.onBtnPrevious} className="link mr-3"><i className="zmdi zmdi-chevron-left zmdi-hc-lg"></i></a>
                        <input id={ `${ this.paginationPrefixId }currentPageTwo` } type="text" className="browser-default customInputs center-align mr-1 pagerInput validate" onChange={this.currentPageChanged} />
            of<span id={ `${ this.paginationPrefixId }lbTotalPagesTwo` } className='ml-2'></span>
                        <a onClick={this.onBtnNext} className="link ml-2"><i className="zmdi zmdi-chevron-right zmdi-hc-lg"></i></a>
                    </div>
                </div>
            );
        }
        else
            if (gridLength > this.state.paginationPageSize) {               
                return (<div>
                    <div className={this.props.gridColData.pagination ? 'input-field col m2 pagedropdownCount ' : 'hide'}>
                        <select onChange={this.onPageSizeChanged} id={ `${ this.paginationPrefixId }page-size` } className='browser-default'>
                            <option value="10" selected defaultValue>10 Rows</option>
                            <option value="25">25 Rows</option>
                            <option value="50">50 Rows</option>
                            <option value="100">100 Rows</option>
                            <option value="500">500 Rows</option>
                            <option value={gridLength} >Show All</option>
                        </select>
                    </div>
                    <div className={this.props.gridColData.pagination ? "input-field col pr-0 mr-0 pagination" : "hide"}>
                        <a onClick={this.onBtnPrevious} className="link mr-3"><i className="zmdi zmdi-chevron-left zmdi-hc-lg"></i></a>
                        <input id={ `${ this.paginationPrefixId }currentPage` } type="text" className="browser-default customInputs center-align mr-1 pagerInput validate" onChange={this.currentPageChanged} />
                of<span id={ `${ this.paginationPrefixId }lbTotalPages` } className='ml-2'></span>
                        <a onClick={this.onBtnNext} className="link ml-2"><i className="zmdi zmdi-chevron-right zmdi-hc-lg"></i></a>
                    </div>
                </div>);
            }
            else {
                return ("");
            }
    }

    renderExport = () => {
        const gridLength = this.props.gridRowData ? this.props.gridRowData.length : 0;
        if (gridLength > 0) {
            return (<div className={this.props.gridColData.gridActions ? 'input-field col right-left gridExport right' : 'hide'}>
                <img width='24' className="link tooltipped" data-position="left" data-tooltip="Export To CSV" title="Export To CSV" onClick={this.props.exportCSV !==undefined? this.props.exportCSV:this.exportToCSV} src='data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz48IURPQ1RZUEUgc3ZnIFBVQkxJQyAiLS8vVzNDLy9EVEQgU1ZHIDEuMS8vRU4iICJodHRwOi8vd3d3LnczLm9yZy9HcmFwaGljcy9TVkcvMS4xL0RURC9zdmcxMS5kdGQiPjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCI+PHBhdGggZD0iTTEyLDFMOCw1SDExVjE0SDEzVjVIMTZNMTgsMjNINkM0Ljg5LDIzIDQsMjIuMSA0LDIxVjlBMiwyIDAgMCwxIDYsN0g5VjlINlYyMUgxOFY5SDE1VjdIMThBMiwyIDAgMCwxIDIwLDlWMjFBMiwyIDAgMCwxIDE4LDIzWiIgLz48L3N2Zz4=' />
            </div>);
        }
        else {
            return ("");
        }
    }

    clearFilters = () => {
        const fields=this.props.gridColData.columnsFiltersToDestroy;
            fields && fields.forEach(field => { this.gridApi.destroyFilter(field);
            });
        this.setState({ searchtxtbox:"" });
        this.gridApi.setFilterModel(null);
        this.gridApi.onFilterChanged();
        this.gridApi.setQuickFilter(null);
        this.gridApi.setSortModel(null);
        if(this.gridApi != undefined){
            this.props.clearFilters && this.props.clearFilters();
        }
    }

    renderFilters = () => {
        if(this.gridApi != undefined){
            this.props.filterGrid && this.props.filterGrid(this.gridApi.getDisplayedRowCount(),this.gridApi.getSelectedRows());
        }
        const gridLength = this.props.gridRowData ? this.props.gridRowData.length : 0;
        if (this.props.gridColData.clearFilter) {
            if (gridLength > 0) {
                return (<div className={this.props.gridColData.enableFilter ? this.props.gridColData.gridTitlePanel ? 'input-field col right-left gridFilter right' : 'input-field col right-left gridFilter mr-2 right' : 'hide'}>
                    <img width='24' className="link tooltipped" data-position="left" data-tooltip="Clear Filter" title="Clear Filter" onClick={this.clearFilters} src='data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz48IURPQ1RZUEUgc3ZnIFBVQkxJQyAiLS8vVzNDLy9EVEQgU1ZHIDEuMS8vRU4iICJodHRwOi8vd3d3LnczLm9yZy9HcmFwaGljcy9TVkcvMS4xL0RURC9zdmcxMS5kdGQiPjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCI+PHBhdGggZD0iTTE0LjczLDIwLjgzTDE3LjU4LDE4TDE0LjczLDE1LjE3TDE2LjE1LDEzLjc2TDE5LDE2LjU3TDIxLjgsMTMuNzZMMjMuMjIsMTUuMTdMMjAuNDEsMThMMjMuMjIsMjAuODNMMjEuOCwyMi4yNEwxOSwxOS40TDE2LjE1LDIyLjI0TDE0LjczLDIwLjgzTTEzLDE5Ljg4QzEzLjA0LDIwLjE4IDEyLjk0LDIwLjUgMTIuNzEsMjAuNzFDMTIuMzIsMjEuMSAxMS42OSwyMS4xIDExLjMsMjAuNzFMNy4yOSwxNi43QzcuMDYsMTYuNDcgNi45NiwxNi4xNiA3LDE1Ljg3VjEwLjc1TDIuMjEsNC42MkMxLjg3LDQuMTkgMS45NSwzLjU2IDIuMzgsMy4yMkMyLjU3LDMuMDggMi43OCwzIDMsM1YzSDE3VjNDMTcuMjIsMyAxNy40MywzLjA4IDE3LjYyLDMuMjJDMTguMDUsMy41NiAxOC4xMyw0LjE5IDE3Ljc5LDQuNjJMMTMsMTAuNzVWMTkuODhNNS4wNCw1TDksMTAuMDZWMTUuNThMMTEsMTcuNThWMTAuMDVMMTQuOTYsNUg1LjA0WiIgLz48L3N2Zz4=' />
                </div>);
            }
            else {
                return ("");
            }
        }

    }

    renderGridRefresh = () => {
        if (this.props.gridColData && this.props.gridColData.gridRefresh){
            return (<a  onClick={this.props.gridRefreshHandler} className="right mt-3 mr-2 btn-small bold"><i className="zmdi zmdi-refresh zmdi-hc-lg"></i></a>);
        } else {
            return null;
        }
    };

    renderGridLoadMore = (data) => {
        if (Array.isArray(this.props.gridRowData) && this.props.gridRowData.length > 0 && this.props.totalCount && this.props.totalCount > this.props.gridRowData.length) {
            return (<a onClick={this.props.loadMoreClick} className="right mt-3 mr-2 btn-small bold" title={this.props.gridRowData.length +" of "+this.props.totalCount}>Load More ({this.props.gridRowData.length +" of "+this.props.totalCount})</a>);
        } else {
            return null;
        }
    };

    getNodeChildDetails(rowItem) {
        if (this.props.isGrouping) {
            if (rowItem[this.props.dataName]) {
                return {
                    group: true,
                    expanded: this.props.expanded !==undefined ?this.props.expanded: true,
                    children: rowItem[this.props.dataName],
                    key: rowItem[this.props.groupName]
                };
            } else {
                return null;
            }
        }
        else {
            return null;
        }
    }
    onCellchange = (param) => {    
        this.props.onCellchange(param);  
    }
    setData=(newData,nodeIndex)=>{
       const rowDataid = this.gridApi.getRowNode(nodeIndex);
       rowDataid.setData(newData);
    }    
    //To referesh Grid after an update.
    refreshGrid =() =>{
        this.gridApi.redrawRows();
    }
    // setCurrentPage=(text)=> {            
    //      this.setState({ txtCurrentPage:text });      
    // };
    // setTotalPage=(text)=> {
    //        this.setState({ lbTotalPages:text });           
    //     };
    // setTotalPageTwo=(text)=> {
    //         this.setState({ lbTotalPagesTwo:text });           
    //         }
            
    // setCurrentPageTwo=(text)=> {
    //         this.setState({ txtCurrentPageTwo:text });          
    //      }

    render() {
        const gridLength = this.props.gridRowData ? this.props.gridRowData.length : 0;
        const noRowTemplate = (Array.isArray(this.props.gridRowData) && this.props.gridRowData.length === 0) ? this.state.overlayNoRowsTemplate : this.state.overlayLoadingTemplate;
        const rowHeight = this.props.gridColData.rowHeight ? this.props.gridColData.rowHeight : "30";
 
        return (
            <Fragment>

                <div className={this.props.gridColData.gridTitlePanel ? 'row m-0' : this.props.gridColData.enableFilter ? 'row m-0' : 'hide'}>
                    {
                        this.renderGridSearchBox()
                        
                    }
                    {
                        this.renderPagination()
                    }
                    {

                        this.renderExport()

                    }

                    {
                        this.renderFilters()
                    }
                    { this.renderGridRefresh() }

                    {
                        this.renderGridLoadMore()
                    }
                </div>
                {/* <div className="ag-theme-material reactAgGrid" style={{ "height": this.props.gridColData.gridHeight + "vh" }}> */}
                <div id={this.props.paginationPrefixId} className={this.props.gridCustomClass ? this.props.gridCustomClass + " ag-theme-material reactAgGrid" : "ag-theme-material reactAgGrid"}>
                    <AgGridReact
                        columnDefs={this.props.gridColData.columnDefs}
                        getNodeChildDetails={this.getNodeChildDetails}
                        rowData={this.props.gridRowData}
                        
                        enableFilter={this.props.gridColData.enableFilter}
                        multiSortKey={this.props.gridColData.multiSortKey}
                        enableSorting={this.props.gridColData.enableSorting}
                        pagination={this.props.gridColData.pagination}
                        paginationPageSize={this.state.paginationPageSize}
                        suppressPaginationPanel={true}
                        suppressMenuHide={true}
                        enableColResize={true}
                        rowClass="customRow"
                        rowHeight={rowHeight}
                        rowClassRules={this.state.rowClassRules}
                        frameworkComponents={this.state.frameworkComponents}
                        suppressRowClickSelection={this.props.gridColData.suppressRowClickSelection ? false : true}
                        rowSelection={this.props.gridColData.rowSelection}
                        overlayLoadingTemplate={this.state.overlayLoadingTemplate}
                        overlayNoRowsTemplate={noRowTemplate}
                        suppressDragLeaveHidesColumns={true}
                        defaultColDef={this.state.defaultColDef}
                        groupSelectsChildren={true}
                        suppressAggFuncInHeader={true}
                        animatedRows={this.props.gridColData.animatedRows}
                        rowDragManaged={this.props.gridColData.rowDragManaged}
                        domLayout='autoHeight'
                        onRowDragEnd={this.props.onRowDragEnd}
                        onRowSelected={this.props.rowSelected}
                        onRowDataChanged={this.onRowDataChanged}
                        onPaginationChanged={this.onPaginationChanged}
                        onGridReady={this.onGridReady}
                        onSelectionChanged={this.props.onSelectionChanged}
                        onRowDoubleClicked={this.props.onRowDoubleClicked}
                        onCellValueChanged={this.onCellchange}
                        onCellFocused={this.props.onCellFocused}
                        onCurrentPageChanged={this.props.onCurrentPageChanged}
                        pinnedTopRowData={this.props.gridColData.pinnedTopRowData}   
                        filterGrid={this.props.filterGrid}   
                        clearFilters={this.props.clearFilters}                                
                        headerHeight={this.props.gridColData.headerHeight ? this.props.gridColData.headerHeight : 40} >
                    </AgGridReact>
                </div>
            </Fragment>
        );
    }
}
function setTotalPage(text,prefixId) {
    const element = document.querySelector(`#${ prefixId }lbTotalPages`);
    if (element !== undefined && element !== null) {
        element.innerHTML = text;
    }
}
function setCurrentPage(text,prefixId) {
    const element = document.querySelector(`#${ prefixId }currentPage`);
    if (element !== undefined && element !== null) {
        element.value = text;       
    }
}
function setCurrentPageTwo(text,prefixId) {
    const element = document.querySelector(`#${ prefixId }currentPageTwo`);
    if (element !== undefined && element !== null) {
        element.value = text;        
    }    
}
function setTotalPageTwo(text,prefixId) {
    const element = document.querySelector(`#${ prefixId }lbTotalPagesTwo`);
    if (element !== undefined && element !== null) {
        element.innerHTML = text;
    }
}
export default connect(null, { AddNewInvoiceDefault })(ReactGrid);