import React, { Fragment } from 'react';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData } from '../../../../utils/commonUtils';

import { CheckPermission } from '../../../../utils/permissionUtil';
const localConstant = getlocalizeData();

const saveBar = (props) => {    
    return (
        <Fragment>
            <div className="customCard row saveBar_fixed">
                <div className="col s2 left-align pl-0 pt-1">
                    <strong className="bold">{props.currentMenu}: </strong> <span>{props.currentSubMenu}</span>
                </div>
                <div className="col s2 pt-1 pinWidth">
                    <strong className="bold">{props.codeLabel}: </strong> <span>{props.codeValue}</span> <br/> 
                    <strong className="bold left">{props.resoureceLableName}</strong> <span className="resourceNameShort">{ ` ${ props.techSpecInfo.firstName ? props.techSpecInfo.firstName : "" } ${ props.techSpecInfo.lastName ? props.techSpecInfo.lastName :"" }` } </span>
                </div>
                    <div className="col s2 pr-0 pl-0 sendToParent">
                    <CustomInput
                            hasLabel={true}
                            divClassName='pr-0 pl-0 sendtoSelect'
                            label={localConstant.techSpec.common.SEND_TO}
                            labelClass='sendtoLabel bold'
                            type='select'
                            colSize='s12 mb-0'
                            className="browser-default"
                            optionName='name'
                            optionValue="name"
                            optionsList={props.sendRCRM  ? props.sendtoInfo : props.sendtoInfoWthOutRCRM} //Changes For Sanity Def 116
                            name='profileAction'
                            onSelectChange={props.sendToOnChangeHandler}
                            defaultValue = {props.techSpecInfo.profileAction}
                            disabled={!props.ShowSendtoInfo || props.interactionMode} />
                    </div>
                {props.techSpecInfo.profileAction === localConstant.techSpec.common.SEND_TO_TM?
                <div className="col s2 pr-0 pl-0 listOfTM_Width">
                    <CustomInput
                        hasLabel={true}
                        divClassName='pr-0 listOfTMSelect'
                        label={'List of TM: '}
                        labelClass='sendtoLabel bold'
                        type='select'
                        colSize='s12 mb-0 pl-1'
                        className="browser-default"
                        optionName='userName'
                        optionValue="logonName"
                        optionsList={props.listOfTM}
                        name='assignedToUser'
                        onSelectChange={props.technicalManagerOnChangeHandler}
                        defaultValue = {props.techSpecInfo.assignedToUser}
                        disabled={!props.ShowSendtoInfo} />
                </div>:null
                }
                {/** Started for D946 DR */}
                 { props.showListOfRC ?
                <div className="col s2 pr-0 pl-0 listOfTM_Width">
                    <CustomInput
                        hasLabel={true}
                        divClassName='pr-0 listOfTMSelect'
                        label={localConstant.techSpec.common.LIST_OF_RC}
                        labelClass='sendtoLabel bold'
                        type='select'
                        colSize='s12 mb-0 pl-1'
                        className="browser-default"
                        optionName='userName'
                        optionValue="logonName"
                        optionsList={props.listOfRC}
                        name='assignedToRCUser'
                        onSelectChange={props.technicalManagerOnChangeHandler}
                        defaultValue = {props.techSpecInfo.assignedToUser}
                        disabled={!props.showListOfRC} />
                </div>:null
                }
                {/** End for D946 CR */}
                <div className={props.techSpecInfo.profileAction === localConstant.techSpec.common.SEND_TO_TM ? 'col s3 right-align mb-0 pr-0 pl-0 right listofBtn' :'col right-align mb-0 pr-0 pl-0 right ' }>
                 {
                    props.buttons.map((eachButton, index) => { 
                        const hasPermission = CheckPermission(eachButton.permissions, props.activities);  
                        let result = null;
                        if(hasPermission===true){
                            if(  eachButton.showBtn === undefined || eachButton.showBtn === true){
                                if(eachButton.name === localConstant.commonConstants.SAVE_AS_DRAFT){
                                    return result= <button className={eachButton.className} key={index} onClick={eachButton.clickHandler()} disabled={props.isbtnDisableDraft === true ? props.isbtnDisableDraft : props.isbtnDisable }>{eachButton.name}</button>;                        
                                }else if (eachButton.showBtn) {
                                    return  result=<button className={eachButton.className} key={index} onClick={(e)=>eachButton.clickHandler(e)} disabled={ eachButton.isbtnDisable }>{eachButton.name}</button>;
                                }else  
                                return result=<button className={eachButton.className} key={index} onClick={eachButton.clickHandler()} disabled={eachButton.isbtnDisable }>{eachButton.name}</button>;                                            
                            }
                        }
                          return result; 
                    })
                }
            </div>
            </div>
        </Fragment>
    );
};

export default saveBar;