/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/applications/queries';
import { SemanticVersion } from './SemanticVersion';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/cratis/versions');

export class CratisVersions extends QueryFor<SemanticVersion[]> {
    readonly route: string = '/api/cratis/versions';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: SemanticVersion[] = [];

    constructor() {
        super(SemanticVersion, true);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    static use(): [QueryResultWithState<SemanticVersion[]>, PerformQuery] {
        return useQuery<SemanticVersion[], CratisVersions>(CratisVersions);
    }
}
