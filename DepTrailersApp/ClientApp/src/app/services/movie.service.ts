import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { Movie } from '../models/movie';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  myAppUrl: string;
  myApiUrl: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  constructor(private http: HttpClient) {
    this.myAppUrl = environment.appUrl;
    this.myApiUrl = 'api/movies/';
  }

  getPopularMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.myAppUrl + this.myApiUrl+ 'popular/')
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  getSearchResults(query: string): Observable<Movie[]> {

    //if options
    /*const options = term ?
      { params: new HttpParams().set('name', term) } : {};*/

    const searchparam = query ?
      { params: new HttpParams().set('q', query) } : {};
    return this.http.get<Movie[]>(this.myAppUrl + this.myApiUrl + 'find', searchparam)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }
  getMovie(id: number): Observable<Movie> {
    return this.http.get<Movie>(this.myAppUrl + this.myApiUrl + id)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  errorHandler(error) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
