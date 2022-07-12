import React, { Fragment } from 'react';
import { CheckPermission } from '../../../utils/permissionUtil';
const saveBar = (props) => {
    return (
        <Fragment>
            <div className="customCard row saveBar_fixed">
            <div className="col s2 left-align pl-0 pt-1">
       
                <strong className="bold">{`${ props.currentMenu } : `} </strong> <span>{props.currentSubMenu}</span>
            </div>
            <div className="col s pt-1">
      
          <strong className="bold">{`${ props.codeLabel }: `} </strong> <span>{props.codeValue}</span>
         </div>
                <div className="col s8 right-align mb-0 pr-0">
                    <button className="btn-small mr-0 ml-2" onClick={props.saveClick} disabled={props.isbtnDisable}>Save</button>
                    <button className="btn-small mr-0 waves-effect ml-2 modal-trigger" onClick={props.cancelClick} disabled={props.isbtnDisable}>Cancel</button>
                    {props.currentMenu === "Contracts" ? <button
                        className={props.currentPage === "Create Contract" ? "hide" : "btn-small ml-2 waves-effect mr-0 modal-trigger"}
                        onClick={props.deleteClick} disabled={(props.currentSubMenu === "Edit Contract") ? false : true}>Delete</button> : null}
                </div>
            </div>
        </Fragment>
    );
};

export default saveBar;

export const SaveBarWithCustomButtons = (props) => {
    return (
        <div className="customCard row saveBar_fixed right">
            <div className={ props.currentMenuClass ? `${ props.currentMenuClass } left-align pl-0 pt-1 pr-0` : "col s12 m4 left-align pl-0 pt-1 pr-0" }>
                <strong className="bold">{`${ props.currentMenu } : `}</strong> <span>{props.currentSubMenu}</span>
                </div>
                {
                    (props.saveBarText && Array.isArray(props.saveBarText)) ?
                    props.saveBarText && props.saveBarText.map(iteratedValue => {
                        return (<div className={iteratedValue.codeLabelDivClass?iteratedValue.codeLabelDivClass:"col s12 m3 pt-1"}>
                        <span className="textNoWrapEllipsis" title={iteratedValue.codeValue}><strong className="bold">{`${ iteratedValue.codeLabel }: `}</strong> {iteratedValue.codeValue}</span>
                        </div>);
                    })
                    : props.codeLabel?
                    <div className={props.codeLabelDivClass?props.codeLabelDivClass:"col s12 m3 pt-1"}>
                        <span className="textNoWrapEllipsis" title={props.codeValue}><strong className="bold">{`${ props.codeLabel }: `}</strong> {props.codeValue}</span>
                    </div>:null
                }

            <div className={ props.isbuttonDiv === true ?  "col s12 m12 right-align mb-0 pr-3 right" : 'col s12 m5 right-align mb-0 pr-3 right'}>
            {
                props.buttons.map((eachButton, index) => {
                    const hasPermission = CheckPermission(eachButton.permissions, props.activities);
                    let result = null;
                    if (hasPermission === true) {
                        if ( eachButton.showBtn === undefined || eachButton.showBtn === true)  {
                            result = <button className={eachButton.className}
                                key={index}
                                onClick={(e) => eachButton.clickHandler(e)}
                                disabled={eachButton.isbtnDisable || props.isARSSearch}>{eachButton.name}</button>;
                        }
                    }
                    return result;
                })
            }
        </div>
        </div>
    );
};