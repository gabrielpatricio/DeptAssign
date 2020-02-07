import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormControl } from '@angular/forms';
import {MovieService} from '../services/movie.service';
import { Movie } from '../models/movie';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  query = new FormControl('');
  results$: Movie[] = [];
  mySubscription: any;


  constructor(private route: ActivatedRoute, private router: Router, private movieService: MovieService) {
    router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    };
    this.mySubscription = router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        // Trick the Router into believing it's last link wasn't previously loaded
        router.navigated = false;
      }
    });
  }
  
  /**
   * Method called everytime input on search bar changes
   *
   * DebounceTime: Each time the input value changes, a request is performed
   * when there haven’t been any other values for 200 ms.
   *
   * DistinctUntilChanged: Ignore serial duplicates
   *
   * Switchmap: Guarantees in-order results
   * **/
  ngOnInit() {
    this.query.valueChanges
      .pipe(debounceTime(200),
        distinctUntilChanged(),
        switchMap((query) => this.movieService.getSearchResults(query)))
      .subscribe(result => {
        if (result === []) { return; }
        else { this.results$ = result; }
      });
    
  }
  /**
   * Workaround to reload the component
   * if query string changes
   * */
  ngOnDestroy() {
    if (this.mySubscription) {
      this.mySubscription.unsubscribe();
    }
  }
  /*
   * Method called if button search is triggered,
   * query parameter is obtained from search bar input 
   * **/
  search() {
        this.results$ = [];
        this.router.navigate(['/search'], { queryParams: { q: this.query.value } 
      });
    
  }
  gotoMovie(id) {
    this.results$ = [];
    this.router.navigate(['/movie/'+id]);
  }
  gotoHome() {
    this.results$ = [];
    this.router.navigate(['/']);
  }

}
