/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { Settings } from './Settings';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organization/settings');

export class AllSettings extends QueryFor<Settings> {
    readonly route: string = '/api/organization/settings';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Settings = {} as any;

    constructor() {
        super(Settings, false);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    static use(): [QueryResultWithState<Settings>, PerformQuery] {
        return useQuery<Settings, AllSettings>(AllSettings);
    }
}
