import React from 'react';
import { required } from '../../../utils/validator';

const LabelWithValue =({ className,colSize,label,value, isValidation, validationMsg } = this.props)=>{
    return(
        <div className={'col '+ colSize +' '+ className} title={value}>
            { (isValidation && !required(validationMsg)) && 
                <span class="text-red grid-error-icon_font" title={validationMsg}><i class="zmdi zmdi-alert-triangle grid-mandate-triangle"></i></span>
            }
            <label className='customLabel'>{label}</label> <span>{value} </span>
        </div>
         );
    };

export default LabelWithValue;