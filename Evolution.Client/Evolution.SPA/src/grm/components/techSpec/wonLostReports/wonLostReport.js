import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Panel from '../../../../common/baseComponents/panel';
import  LabelWithValue  from '../../../../common/baseComponents/customLabelwithValue';
import { HeaderData } from './headerData';
import { getlocalizeData,isEmpty } from '../../../../utils/commonUtils';
import { wonLostPdfHeader } from './pdfHeader';
import moment from 'moment';
import jsPDF from 'jspdf';
import PropTypes from 'prop-types';
import 'jspdf-autotable';
import { formInputChangeHandler } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();

/** wonLost SearchFilter starts */
const WonLostReportFilter = (props) => (
    <form id ="contractSearch"

        onSubmit={props.searchWonLostReports}
        onReset={props.clearSearchData} autoComplete="off">
        <div className="row mb-0">
        <CustomInput
                hasLabel={true}
                colSize='s3'
                isNonEditDateField={false}
                name="fromDate"
                label={localConstant.techSpec.common.FROM_DATE}
                type='date'
                inputClass="customInputs"
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                selectedDate={props.selectedFromDate}
                onDateChange={props.onFromDateChange}
                shouldCloseOnSelect={true}
                />

            <CustomInput
                hasLabel={true}
                name="ToDate"
                colSize='s3'
                label={localConstant.techSpec.common.TO_DATE}
                type='date'
                inputClass="customInputs"
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                selectedDate={props.selectedToDate}
                onDateChange={props.onToDateChange}
                shouldCloseOnSelect={true}/>

              {/* <CustomInput hasLabel={true}
                name="companyCode"
                id="Company"
                divClassName='col'
                label={localConstant.gridHeader.COMPANY_NAME}
                type='select'
                colSize='s4 pl-0'
                inputClass="customInputs"
                optionsList={props.companyList}
                optionName='companyName'
                optionValue="companyCode"
                className="browser-default"
                onSelectChange={props.onCompanyChange}
            /> */}

           <LabelWithValue
            className="custom-Badge col br1"
            label="Company Name : "
            colSize='s3'
            value={ props.selectedCompany }
            />

            </div>

             <div className="row mb-0">
             {/* <CustomInput hasLabel={true}
                name="assginedTo"
                id="assginedTo"
                label={localConstant.techSpec.common.CORDINATOR_NAME}
                type='select'
                colSize='s4'
                inputClass="customInputs"
                optionsList={props.coordinatorList}
                optionName='displayName'
                optionValue='username'
                className="browser-default"
                onSelectChange={props.handlerChange}
            /> */}
            <CustomInput 
                hasLabel={true}
                name="action"
                id="status"
                label={ localConstant.techSpec.common.STATUS }
                type='select'
                colSize='s3'
                inputClass="customInputs"
                optionsList={props.status}
                optionName='name'
                optionValue="value"
                className="browser-default"
                onSelectChange={props.handlerChange}
            />
            <div ref ={props.dispositionRef}>
            <CustomInput hasLabel={true}
                        name="dispositionType"
                        id="dispositionId"
                        label={ localConstant.resourceSearch.DISPOSITION_DETAILS }
                        type='select'
                        colSize='s3'
                        inputClass="customInputs"
                        optionsList={props.dispositionType}
                        optionName='name'
                        optionValue="code"
                        className="browser-default" 
                        onSelectChange={props.onSelectChange}
                    />
                    </div>
            {/* Commented as field is not required for Client */}
            {/* <CustomInput hasLabel={true}
            name="Description"
            id="des"
            label="Excel Description"
            type='text'
            colSize='s3'
            inputClass="customInputs"
            className="browser-default"
            onValueChange={ props.onDescriptionChange} 
        />  */}      
            <button type="submit" className="mt-4x mr-2 modal-close waves-effect waves-green btn ">Search</button>
            <button type="reset" className="mt-4x modal-close waves-effect waves-green btn" >Reset</button>
            </div>
        
    </form>
);
/**WonLost Search Filter Ends */

/**gridPanel Field starts */
const WonLostGridFields= (props) => (
    <div className="row">

    {/* <CustomInput hasLabel={true}
    name="Description"
    id="des"
    label="Excel Description"
    type='text'
    colSize='s4'
    inputClass="customInputs"
    className="browser-default"
    onValueChange={ props.onDescriptionChange} 
/>  */}

      {/* <LabelWithValue
            className="custom-Badge col br1"
            label="Won Count"
            colSize='s2'
            value={props.wonCount}
        />
       <LabelWithValue
            className="custom-Badge col br1"
            label="Lost Count"
            colSize='s2'
            value={props.lostCount}
        /> */}

        <button className="btn-flat right" colSize='s4' onClick={props.exportPdf}>Export PDF</button>
     </div>
);
/**gridpanel field ends */

class WonLostReport extends Component {
    constructor(props) {
       
        super(props);
        this.state = {
            isPanelOpen: true,
            selectedFromDate:'',
            selectedToDate:'',
            // descriptionChange:''
            
        };
        this.wonLostFilteredData='';
        this.selectedCompanyName='';
        // this.descriptionChange='';
        this.headerData = HeaderData();
        this.wonLostSearchDatas={};

         this.properties={};       
         this.wonLostHeadingText ='';
         this.dispositionRef = React.createRef();
  
    }

    /**to do ComponentDidMount */
    componentDidMount() {
        this.dispositionRef.current.style.cssText="display:none";
    }
    hideDispositionType(){
        this.dispositionRef.current.style.cssText="display:none";
    }

    /**panel click action */
    panelClick = (e) => {
        this.setState((state) => {
            return {
                isPanelOpen: !state.isPanelOpen
            };
        });
    }

    formatReportDate(formatedDate) {
        const finalFormatedDate = moment(formatedDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        const RDate = finalFormatedDate.split('-'); // this fix was given due to firefox date conversion issue
        const RYear = RDate[0] == '' ? RDate[1] : RDate[0];
        const RMonth = RDate[0] == '' ? RDate[2] : RDate[1];
        const RDay = RDate[0] == '' ? RDate[3] : RDate[2];
        return new Date(RYear + '-' + RMonth + '-' + RDay);
    }

    /**
     *  WonLost Search Action
     */
    searchWonLostReports =(e)=>{
        e.preventDefault();
        const toDate = this.wonLostSearchDatas.toDate;
        const fromDate = this.wonLostSearchDatas.fromDate;

        if((!fromDate || fromDate == '') && (toDate && toDate != '')){
            IntertekToaster(localConstant.errorMessages.WON_LOST_DATE_VALIDATE,'warningToast wonlostReport');
            return false;
        }

        if((fromDate && fromDate != '') && (toDate && toDate != '')){
            const date1 = this.formatReportDate(fromDate);
            const date2 = this.formatReportDate(toDate);
            if(date1 > date2){
                IntertekToaster(localConstant.errorMessages.WON_LOST_DATE_COMPARE,'warningToast wonlostReport');
                return false;
            }
        }

        this.wonLostSearchDatas.searchType='ARS';
        this.wonLostSearchDatas.companyCode=this.props.selectedCompany;
        this.props.actions.FetchWonLostDetails(this.wonLostSearchDatas);          
    }
    clearSearchData=()=>{
        this.setState({
            selectedFromDate: '',
            selectedToDate: ''                        
        });
        this.hideDispositionType();
        this.wonLostSearchDatas={};
    }

    /**From Date change function */
    onFromDateChange =(date)=>{

        if (isEmpty(date)) {
            this.setState({
                selectedFromDate: ""
            }, () => {
                this.wonLostSearchDatas.fromDate = undefined;
            });
        }
        else{ 
        this.setState({
            selectedFromDate: moment(date)
        }, () => {
            this.wonLostSearchDatas.fromDate=moment(this.state.selectedFromDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        });
    }

    }

    /**To Date change function */
    onToDateChange =(date)=>{
        if (isEmpty(date)) {
            this.setState({
                selectedToDate: ""
            }, () => {
                this.wonLostSearchDatas.toDate = undefined;
            });
        }
        else{      
        this.setState({
            selectedToDate: moment(date)
        }, () => {
            this.wonLostSearchDatas.toDate=moment(this.state.selectedToDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        });
    }

    }
    
    /** Pdf Export Function */
    exportPdf = () => {
                    const tableheaders = [];
                    const pdfHeaders = [];
                    const rowDatas = [];
                    const data = this.props.wonLostDatas.filter(item=>item.action!=='CUP' && item.action!=='APA' && item.action!=='SS' && item.action!=='N/A');
                    for (let j = 0; j < wonLostPdfHeader.WonLostHeader.length; j++) {
                        tableheaders.push(wonLostPdfHeader.WonLostHeader[j].field);
                    }
                    for (let s = 0; s < wonLostPdfHeader.WonLostHeader.length; s++) {
                        pdfHeaders.push(wonLostPdfHeader.WonLostHeader[s].headerName);
                    }
                    for (let i = 0; i < data.length; i++) {
                        rowDatas[i] = [];
                        for (let k = 0; k < tableheaders.length; k++) {
                        
                            for (const key in data[i]) {
                                if(typeof(data[i][key])!== 'object')
                                {
                                if (tableheaders[k] === key) {
                                    rowDatas[i].push(data[i][key]);
                                }
                            }
                            else{
                                const searchParamDatas=data[i][key];

                                for(const key in searchParamDatas)
                                {
                                    if (tableheaders[k] === key) {

                                        rowDatas[i].push(searchParamDatas[key]);
                                    }
                                }                                                                          
                            }
            
                            }
                        }
                }
             
        const doc = new jsPDF('l', 'pt', 'a1');
        doc.text(this.wonLostHeadingText, 750, doc.autoTableEndPosY() + 30);
        doc.autoTable(
            {
                theme: 'grid',
                head: [ pdfHeaders ],
                body: rowDatas,
                margin: { top: 80 },
                styles: { overflow: 'linebreak', columnWidth:'wrap' },
            }
        );
        const pdfName = 'WonLost.pdf';
        doc.save(pdfName);
    }

    /**Handler change */
    handlerChange = (e) => {
        const inputValue = formInputChangeHandler(e);
        this.wonLostSearchDatas[inputValue.name] = inputValue.value;
        if (inputValue.name === "action" && (inputValue.value === "SD" || inputValue.value === "L")) {
            this.dispositionRef.current.style.cssText = "display:block";
            this.props.actions.FetchDispositionType();
        }
        else if(inputValue.value === "W"){
            this.wonLostSearchDatas['dispositionType'] = undefined;
            this.dispositionRef.current.style.cssText = "display:none";
        }
        else {
            this.wonLostSearchDatas[inputValue.name] = undefined;
            this.wonLostSearchDatas['dispositionType'] = undefined;
            this.dispositionRef.current.style.cssText = "display:none";
        }
    }

    onSelectChange = (e) => {
        const inputValue = formInputChangeHandler(e);
        this.wonLostSearchDatas[inputValue.name] = inputValue.value;
        this.props.actions.FetchDispositionType(inputValue.value);
    }

    /**Oncompany change  */
    onCompanyChange = (e) => {
        
        this.props.actions.FetchCordinators(e.target.value);
        this.wonLostSearchDatas[e.target.name] = e.target.value;
 
    }

    /** Excel Description */
    // onDescriptionChange = (e) => {
    //     this.descriptionChange = e.target.value;
    // }  -- Commented as field is not required for Client

    render() {
     const { companyList, companyCoordinator, wonLostDatas, dispositionType } = this.props;
     const filteredWonLostDatas=wonLostDatas.filter(item=>item.action!=='CUP' && item.action!=='APA' && item.action!=='SS');

        let wonCount=0;
        let lostCount=0;
        let sdCount=0;
        let wonDatas;
        let lostDatas;
        let sdDatas;
        if(this.props.wonLostDatas)
        {
          wonDatas = this.props.wonLostDatas.filter(item => item.action === "W");
          wonCount=wonDatas.length;
          lostDatas =this.props.wonLostDatas.filter(item => item.action === "L");
          lostCount=lostDatas.length;
          sdDatas =this.props.wonLostDatas.filter(item => item.action === "SD");
          sdCount=sdDatas.length;
        }
        // if(this.descriptionChange!=='')    
        // {     
            //   const description = this.descriptionChange?this.descriptionChange:'';    
            //   const won= wonCount ? wonCount:0;
            //   const lost=lostCount?lostCount:0;
            //   const sd=sdCount?sdCount:0; 
            //   const resultData = ` WonCount:  ${ won } LostCount:   ${ lost } SDCount:   ${ sd }`;    
              
            //   this.properties={ "customHeader":resultData };        
            //   this.headerData = HeaderData(this.properties);       
        // }

        const wonC= wonCount ? wonCount:0;
        const lostC=lostCount?lostCount:0;
        const sdC=sdCount?sdCount:0;     
        const result = `WonCount: ${ wonC } LostCount: ${ lostC }  SDCount:   ${ sdC } `; 
        this.properties={ "customHeader":result };        
        this.headerData = HeaderData(this.properties);  
        this.wonLostHeadingText=result; 
        const selectedCompanyCode = this.props.selectedCompany;
        this.props.companyList.forEach(company=>{
            if (company.companyCode === selectedCompanyCode) {
                this.selectedCompanyName = company.companyName;
            }
        });

        return (
            <div className="companyPageContainer customCard">
                <Panel colSize="s12"
                    isArrow={true}
                    heading={`${
                        localConstant.techSpec.common.WON_LOST_REPORT
                        }`}
                    onpanelClick={this.panelClick}
                    isopen={this.state.isPanelOpen} >

                    <WonLostReportFilter
                        onFromDateChange={(e) => this.onFromDateChange(e)}  
                        onToDateChange=  {(e) => this.onToDateChange(e)}   
                        searchWonLostReports={(e) => this.searchWonLostReports(e)}                   
                        clearSearchData={this.clearSearchData}   
                        handlerChange={ (e) => this.handlerChange(e)}
                        onSelectChange={(e)=>this.onSelectChange(e)}
                        dispositionRef={this.dispositionRef}
                        companyList={companyList}
                        status= {localConstant.resourceSearch.quickSearchAction.filter(item=>item.value!=='SS')} //D769 #2 issue
                        onCompanyChange={ (e) => this.onCompanyChange(e)}  
                        coordinatorList={companyCoordinator}    
                        selectedFromDate={this.state.selectedFromDate}
                        selectedToDate={this.state.selectedToDate}
                        selectedCompany={this.selectedCompanyName}
                        dispositionType={dispositionType}
                        // onDescriptionChange={ (e) => this.onDescriptionChange(e) }  -- Commented as field is not required for Client
                    />

                </Panel>

                <div className="customCard"> 

          <WonLostGridFields
                //  onDescriptionChange={ (e) => this.onDescriptionChange(e) }  -- Commented as field is not required for Client
                 wonCount= { wonCount ? wonCount:0}
                 lostCount= { lostCount ? lostCount:0 }
                 exportPdf={this.exportPdf}  
                    />
   
                <ReactGrid
                    gridRowData={ filteredWonLostDatas }
                    gridColData={this.headerData} />
                    </div>
           
            </div>
           
        );
    };
};

export default WonLostReport;

WonLostReport.propTypes = {
    companyList: PropTypes.array,
    companyCoordinator:PropTypes.array,
    wonLostDatas:PropTypes.array

};

WonLostReport.defaultProps = {
    companyList: [],
    companyCoordinator: [],
    wonLostDatas: []
};