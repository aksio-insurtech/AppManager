/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/applications/queries';
import { Application } from './Application';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}');

export interface GetApplicationArguments {
    applicationId: string;
}
export class GetApplication extends QueryFor<Application, GetApplicationArguments> {
    readonly route: string = '/api/applications/{{applicationId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Application = {} as any;

    constructor() {
        super(Application, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    static use(args?: GetApplicationArguments): [QueryResultWithState<Application>, PerformQuery<GetApplicationArguments>] {
        return useQuery<Application, GetApplication, GetApplicationArguments>(GetApplication, args);
    }
}
