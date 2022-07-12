import React from 'react';
import { SearchParameterLabelView,GridView, OptionalParameter,Btn,ExportBtn } from '../resourceFields';
export const SearchGRM = (props) => {
  const { optionalParamLabelData,multiSelectValueChange,multiSelectOptionsList,defaultMultiSelection,optionAttributs,
    equipment,certificatesMultiSelectOptionsList,expiryFrom,expiryTo,searchGRMData,gridGroupProps,searchPanelOpen,isSearchPanelOpen } = props;
      props.gridData.map(item =>
      {
        const selectedRows = item.resourceSearchTechspecInfos.filter( x=> x.isSelected);
        const notSelectedRows = item.resourceSearchTechspecInfos.filter( x=> !x.isSelected);
        item.resourceSearchTechspecInfos = [ ...selectedRows, ...notSelectedRows ];
      });
  return (
    <div className="row mb-0">
        {/* <a className={isSearchPanelOpen ? "toggleSliderOpen" :"toggleSliderClose"} onClick={searchPanelOpen}><i className={isSearchPanelOpen ? "zmdi zmdi-chevron-left" :"zmdi zmdi-chevron-right" }></i></a> */}
        {/* <div className={isSearchPanelOpen ? "col s3 br-1 pr-0 " : 'hide' }>    
        <SearchParameterLabelView
          optionalParamLabelData={optionalParamLabelData}
          interactionMode={props.interactionMode} />
        <OptionalParameter inputChangeHandler={props.inputChangeHandler} isARSSearch={props.isARSSearch}
          equipmentDescriptionMultiselectValueChange={props.equipmentDescriptionMultiselectValueChange}
          certificationMultiSelectValueChange={props.certificationMultiSelectValueChange}
          languageSpeakingMultiSelectValueChange={props.languageSpeakingMultiSelectValueChange}
          languageWritingMultiSelectValueChange={props.languageWritingMultiSelectValueChange}
          languageComprehensionMultiSelectValueChange={props.languageComprehensionMultiSelectValueChange}
          multiSelectOptionsList={multiSelectOptionsList}
          equipment={equipment}
          certificatesMultiSelectOptionsList={certificatesMultiSelectOptionsList}
          defaultMultiSelection={defaultMultiSelection}
          optionAttributs={optionAttributs} 
          defaultCertificateMultiSelection={props.defaultCertificateMultiSelection}
          defaultEquipmentMultiSelection={props.defaultEquipmentMultiSelection}
          defaultLanguageSpeakingMultiSelection={props.defaultLanguageSpeakingMultiSelection}
          defaultLanguageWritingMultiSelection={props.defaultLanguageWritingMultiSelection}
          defaultLanguageComprehensionMultiSelection={props.defaultLanguageComprehensionMultiSelection}
          expiryFrom={expiryFrom}
          expiryTo={expiryTo}
          searchGRMData={searchGRMData}
          defaultCustomerApprovalMultiSelection={props.defaultCustomerApprovalMultiSelection}
          taxonomyCustomerApproved={props.taxonomyCustomerApproved}
          customerApprovalMultiselectValueChange={props.customerApprovalMultiselectValueChange}
          interactionMode={props.interactionMode}
          />        
          <Btn buttons={props.buttons} onClick={props.optionalSearchClick} interactionMode={props.interactionMode}/>
       </div> */}
       <div className={"col s12"}>
          <ExportBtn exportButtons={props.exportButtons} isShowMapBtn={props.isShowMapBtn}/>
          <GridView gridCustomClass={props.gridCustomClass} gridData={props.gridData} headerData={props.gridHeaderData} gridRef={props.gridRef} rowSelected={props.rowSelectedHandler} gridGroupProps={gridGroupProps} isGrouping={true} />
       </div>
       {/* <div className={isSearchPanelOpen ? "col s9 " : 'col s12' }>
          <ExportBtn exportButtons={props.exportButtons} isShowMapBtn={props.isShowMapBtn}/>
          <GridView gridCustomClass={props.gridCustomClass} gridData={props.gridData} headerData={props.gridHeaderData} gridRef={props.gridRef} rowSelected={props.rowSelectedHandler} gridGroupProps={gridGroupProps} isGrouping={true} />
       </div> */}
    </div>
  );
};

export const OptionalSearch = (props) => {
  const { optionalParamLabelData,multiSelectOptionsList,defaultMultiSelection,optionAttributs,
    equipment,certificatesMultiSelectOptionsList,expiryFrom,expiryTo,searchGRMData,isSearchPanelOpen } = props;
     
  return (
    <div className="row mb-0">      
       <div className={"col s12 br-1 pr-0 "}>    
        <SearchParameterLabelView
          optionalParamLabelData={optionalParamLabelData}
          interactionMode={props.interactionMode} />
        <OptionalParameter inputChangeHandler={props.inputChangeHandler} isARSSearch={props.isARSSearch}
          equipmentDescriptionMultiselectValueChange={props.equipmentDescriptionMultiselectValueChange}
          certificationMultiSelectValueChange={props.certificationMultiSelectValueChange}
          languageSpeakingMultiSelectValueChange={props.languageSpeakingMultiSelectValueChange}
          languageWritingMultiSelectValueChange={props.languageWritingMultiSelectValueChange}
          languageComprehensionMultiSelectValueChange={props.languageComprehensionMultiSelectValueChange}
          multiSelectOptionsList={multiSelectOptionsList}
          equipment={equipment}
          certificatesMultiSelectOptionsList={certificatesMultiSelectOptionsList}
          defaultMultiSelection={defaultMultiSelection}
          optionAttributs={optionAttributs} 
          defaultCertificateMultiSelection={props.defaultCertificateMultiSelection}
          defaultEquipmentMultiSelection={props.defaultEquipmentMultiSelection}
          defaultLanguageSpeakingMultiSelection={props.defaultLanguageSpeakingMultiSelection}
          defaultLanguageWritingMultiSelection={props.defaultLanguageWritingMultiSelection}
          defaultLanguageComprehensionMultiSelection={props.defaultLanguageComprehensionMultiSelection}
          expiryFrom={expiryFrom}
          expiryTo={expiryTo}
          searchGRMData={searchGRMData}
          defaultCustomerApprovalMultiSelection={props.defaultCustomerApprovalMultiSelection}
          taxonomyCustomerApproved={props.taxonomyCustomerApproved}
          customerApprovalMultiselectValueChange={props.customerApprovalMultiselectValueChange}
          interactionMode={props.interactionMode}
          />        
          <Btn buttons={props.buttons} onClick={props.optionalSearchClick} interactionMode={props.interactionMode}/>
       </div>
    
    </div>
  );
};