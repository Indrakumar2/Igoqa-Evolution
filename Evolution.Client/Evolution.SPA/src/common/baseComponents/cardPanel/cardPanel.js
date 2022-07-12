import React, { Component,Fragment } from 'react';

class CardPanel extends Component {    
    render() {
        const { className, colSize, title,terms,titleClass, children } = this.props;
        return (
            <Fragment>
            {title && <h6 className={titleClass ? titleClass : "pl-0 bold"}><span>{title}</span></h6> } 
            {terms && <p className="right-align mr-5 danger-txt">{terms}</p>}
            <div className="row mb-1">
            <div className={"col " + colSize}>
              <div className={"card-panel "+ className}>
                {children}  
              </div>
             </div>
             </div>
            </Fragment>
        );
    }
}

export default CardPanel;