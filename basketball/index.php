<html>
	<head>
	<title>NBA Player Stats</title>
	</head>	
	<style type="text/css">
		.search img {
			margin: 0px auto;
			display: block;
		}
		.stats {
			margin: 0px auto;
			display: block;
		}
		.search form {
			text-align: center;
		}
		h2 {
			text-align: center;
		}
		table, th, td {
			border: 1px solid black;
			border-collapse: collapse;
		}
	</style>
	<body>
		<div class="search">
			<img src="img/nba.jpg" alt="logo" style="width:300;height:80px;"/>
			<form method="post">
				<input type="text" name="search" placeholder="search for players..."/>
				<input type="submit" name="submit" value=">>"/><br/>
			</form>
		</div>
		<?php
			header("Content-Type: text/html; charset=ISO-8859-1");
			
			class Stats {
			
				function displayInfo() {

					if(isset($_POST['submit'])) {
					
						$mysqli = new mysqli("localhost", "root", "", "csv_db");
						$search =$mysqli->real_escape_string($_POST['search']);
						$result = $mysqli->query("SELECT * FROM bball WHERE Player LIKE '%$search%'");
						if($result->num_rows > 0) {
							while($row = $result->fetch_assoc()) {
								
								$name  = $row['Player'];
								$team = $row['Team'];
								$pts = $row['points'];
								$reb = $row['Total'];
								$ast = $row['assist'];
								$stl = $row['steals'];
								$blk = $row['blocks'];
								$fgp = $row['fgp'];
								$ptp = $row['ptp'];
				
								echo "<div> 
										<h2>$name</h2> 
										<table align='center'>
											<tr>
												<th>Team</th>
												<th>Pts</th>
												<th>Rebounds</th>
												<th>Assits</th>
											</tr>
											<tr>
												<td>$team</td>
												<td>$pts</td>
												<td>$reb</td>
												<td>$ast</td>
											
											</tr>
											<tr>
												<th>Steals</th>
												<th>Blocks</th>
												<th>FG%</th>
												<th>3P%</th>
											</tr>
											<tr>
												<td>$stl</td>
												<td>$blk</td>
												<td>$fgp</td>
												<td>$ptp</td>
											
											</tr>
										</table>
								
									</div>";							
							}
						}else {
							echo "no results";
						}
					}
				}
			}
			echo(Stats::displayInfo());
		?>
	</body>
</html>