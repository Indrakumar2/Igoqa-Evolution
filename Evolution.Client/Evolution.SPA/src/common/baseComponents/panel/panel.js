import React, { Component,Fragment } from 'react';
import PropTypes from 'prop-types';

class Panel extends Component {    
    render() {
        const { className, colSize, subtitle,heading, children, isopen,onpanelClick,isArrow } = this.props;
        return (
            <Fragment>
            <div className={"col " + colSize +' '+ className}>
            <ul className="collapsible">                   
                    <li className={isopen ? 'active':'inactive'} >
                        <div className="collapsible-header" onClick={onpanelClick}>
                        {heading && <Fragment> <strong className="bold">{heading} </strong> 
                        {subtitle && <span className="pl-1"> {subtitle}</span>}
                        { isArrow ? (isopen ? <i className="zmdi zmdi-chevron-up upDownArrow"></i>:<i className="zmdi zmdi-chevron-down upDownArrow"></i>) :<i className="zmdi zmdi-search searchIcon"></i>}
                        </Fragment>
                        }                
                        </div>
                         <div className="collapsible-body">
                         {children}
                        </div>
                    </li>
            </ul>
        </div>
        </Fragment>
        );
    }
}

export default Panel;

Panel.propTypes = {
    className: PropTypes.string,
    title:PropTypes.string,
    heading:PropTypes.string,
    isopen:PropTypes.bool,
    isArrow:PropTypes.bool
};
Panel.defaultProps = {
    className:'',
    title:'',
    heading:'',
    isArrow:false,
    isopen:false
    
}; 