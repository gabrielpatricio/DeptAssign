import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { MovieService } from '../services/movie.service';
import { Observable } from 'rxjs';
import { Movie } from '../models/movie';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.scss']
})
export class SearchResultsComponent implements OnInit {

  results$: Observable<Movie[]>;
  mySubscription: any;

  constructor(private movieService: MovieService, private route: ActivatedRoute, router: Router) {

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

  ngOnInit() {
    this.loadSearchResults();
    
  }

  ngOnDestroy() {
    if (this.mySubscription) {
      this.mySubscription.unsubscribe();
    }
  }
  loadSearchResults() {
    this.results$ = this.movieService.getSearchResults(this.route.snapshot.queryParamMap.get('q'));
  }

}
