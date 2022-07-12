import React, { Fragment } from 'react';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData } from '../../../../utils/commonUtils';
import LabelWithValue from '../../../../common/baseComponents/customLabelwithValue';

const localConstant = getlocalizeData();

const saveBar = (props) => {    
    return (
        <Fragment>
            <div className="customCard row saveBar_fixed">
                <div className="col s3 left-align pl-0 pt-1">
                    <strong className="bold">{props.currentMenu} </strong> {props.currentSubMenu && <span> : {props.currentSubMenu}</span>}
                </div>

                <div className="col s3 right-align mb-0 pr-0 pl-0 right buttonWidth">
                    {
                     props.buttons?
                        
                        props.buttons.map((eachButton, index) => {
                            if (eachButton.showBtn) {
                                return <button className={eachButton.className}  disabled={eachButton.isDisabled}  key={index} onClick={eachButton.clickHandler()}>{eachButton.name}</button>;
                            }
                        }):null
                    }
                </div>
                { props.childComponents }
            </div>
        </Fragment>
    );
};

export default saveBar;