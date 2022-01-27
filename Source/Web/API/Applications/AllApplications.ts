/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResult, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { Application } from './Application';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications');

export class AllApplications extends ObservableQueryFor<Application[]> {
    readonly route: string = '/api/applications';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Application[] = [];
    readonly requiresArguments: boolean = false;

    static use(): [QueryResult<Application[]>] {
        return useObservableQuery<Application[], AllApplications>(AllApplications);
    }
}
